import { Component } from "@angular/core";
import { LoginService } from 'src/app/services/login.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    constructor(private readonly _loginService: LoginService) {}

    name: string;
    onSubmit() {
        this._loginService.login(this.name ? this.name : 'Jos');
    }

}