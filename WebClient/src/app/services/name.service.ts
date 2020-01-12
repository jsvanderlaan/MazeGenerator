import { Injectable } from "@angular/core";
import { JOS_NAMES, NICKNAMES } from '../constants/nicknames.constants';

@Injectable({
    providedIn: 'root'
})
export class NameService {
    private transform = (arr: {name: string, nicknames: string[]}[]) => (value: string ) => {
        const distinct = (v, i, s) => s.indexOf(v) === i;
        const validNicknames = [];
        const name = value.trim().replace(' ', '').replace('-', '').toLowerCase();
        arr.forEach(nick => nick.nicknames.forEach(n => n.includes(name) && validNicknames.push(nick.name)));
        return validNicknames.filter(distinct);
    }

    getJosNames = this.transform(JOS_NAMES);
    getNames = this.transform(NICKNAMES);
}