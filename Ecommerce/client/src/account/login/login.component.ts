import { Component } from '@angular/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  title="Login"
  constructor(private accountService:AccountService){}
  login(){
    this.accountService.login()
  }
}
