import { Component } from "@angular/core";
import { LoginService } from 'src/app/services/login.service';
import { NameService } from 'src/app/services/name.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    constructor(private readonly _loginService: LoginService, private readonly _nameService: NameService) {}

    name: string;
    onSubmit() {
        this._loginService.login(this._nameService.getNames(this.name)[0]);
    }

}