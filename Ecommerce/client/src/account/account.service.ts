import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Constants } from './constants';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

// We need to have something which won't emit initial value rather wait till it has something.
  // Hence for that ReplaySubject. I have given to hold one user object and it will cache this as well
  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private manager = new UserManager(getClientSettings());
  private user: User | null=null;
  token = "";
  access_token = "";

  constructor(private http: HttpClient, private router: Router) {
    this.manager.getUser().then(user => {
      this.setUser(user);
    });
  }

  private setUser(user: User | null) {
    this.user = user;
    if (user && !user.expired) {
      this.token = user.token_type;
      this.access_token = user.access_token;
      const userName = (user.profile as any)?.name || (user.profile as any)?.preferred_username || (user.profile as any)?.sub || 'alice';
      localStorage.setItem('basket_userName', userName);
      this.currentUserSource.next(true);
    } else {
      this.token = '';
      this.access_token = '';
      this.currentUserSource.next(false);
    }
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  getUserName(): string {
    if (this.user && this.user.profile) {
      return (this.user.profile as any).name || (this.user.profile as any).preferred_username || (this.user.profile as any).sub || 'alice';
    }
    return localStorage.getItem('basket_userName') || 'alice';
  }

  login() {
    return this.manager.signinRedirect();
  }

  async signout() {
    await this.manager.signoutRedirect();
  }

  get authorizationHeaderValue(): string {
    if (this.user && !this.user.expired) {
      return `${this.user.token_type} ${this.user.access_token}`;
    }
    if (this.token && this.access_token) {
      return `${this.token} ${this.access_token}`;
    }
    return '';
  }

  logout() {
    localStorage.removeItem('token');
    this.setUser(null);
    this.signout();
  }

  public finishLogin = (): Promise<User> => {
    return this.manager.signinRedirectCallback()
    .then(user => {
      this.setUser(user);
      return user;
    });
  }

  public finishLogout = () => {
    this.setUser(null);
    return this.manager.signoutRedirectCallback();
  }

  private checkUser = (user : User): boolean => {
    return !!user && !user.expired;
  }

}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`,
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: `${Constants.clientRoot}/signin-callback`,
    scope: "openid profile eshoppinggateway",
    response_type: "code",
    post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
  };
}
