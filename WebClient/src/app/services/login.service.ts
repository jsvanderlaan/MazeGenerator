import { Injectable } from "@angular/core";
import { ReplaySubject, BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoginService {
    private readonly usernameStorageKey = 'username';
    private username$ = new BehaviorSubject<string>(null);
    private loggedIn$ = new BehaviorSubject<boolean>(false);

    constructor() {
        const storedName = localStorage.getItem(this.usernameStorageKey);
        if(storedName) {
            this.login(storedName);
        }
    }

    login(name: string) {
        this.username$.next(name);
        this.loggedIn$.next(true);
        localStorage.setItem(this.usernameStorageKey, name);
    }

    logout() {
        this.username$.next(null);
        this.loggedIn$.next(false);
        localStorage.removeItem(this.usernameStorageKey)
    }

    getUsername$ = () => this.username$.asObservable();
    isLoggedIn$ = () => this.loggedIn$.asObservable();
}