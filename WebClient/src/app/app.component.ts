import { Component, OnInit } from '@angular/core';
import { LoginService } from './services/login.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  loggedIn$: Observable<boolean>;
  constructor(private readonly _loginService: LoginService) {}

  ngOnInit() {
    this.loggedIn$ = this._loginService.isLoggedIn$();
  }

  result: any;
  onResult(result: any) {
    this.result = result
  }
}
