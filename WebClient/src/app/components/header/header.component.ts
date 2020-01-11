import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  username$;
  isLoggedIn$;

  constructor(private readonly _loginService: LoginService) { }

  ngOnInit() {
    this.isLoggedIn$ = this._loginService.isLoggedIn$();
    this.username$ = this._loginService.getUsername$();
  }

  logout = () => this._loginService.logout();

}
