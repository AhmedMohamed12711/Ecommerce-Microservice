import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AccountService } from "../account.service";

@Component({
    selector: 'app-signin-redirect-callback',
    standalone: false,
    template: `<div class="d-flex justify-content-center align-items-center my-5"><div class="spinner-border text-primary" role="status"></div></div>`
})
export class SigninRedirectCallbackComponent implements OnInit {
    constructor(private _router: Router, private acntService: AccountService) { }

    ngOnInit(): void {
        this.acntService.finishLogin()
            .then(_ => {
                this._router.navigateByUrl('/checkout');
            })
            .catch(err => {
                console.error('Error during finishLogin:', err);
                this._router.navigateByUrl('/checkout');
            });
    }
}