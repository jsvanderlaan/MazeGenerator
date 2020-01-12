import { Directive } from '@angular/core';

import { Validator, FormControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[validName][ngModel]',
    providers: [
      { provide: NG_VALIDATORS, useExisting: NameValidatorDirective, multi: true }
    ]
  })
  export class NameValidatorDirective implements Validator { 

    private josNames =[{
        name: 'Jos',
        nicknames: [
            'jos',
            'johooos',
            'joss'
        ]
    }]

    private nicknames = [
        {
            name: 'Pam',
            nicknames: [
                'pamhoogstraaten',
                'pammelahoogstraaten',
                'phoogstraaten'
            ]
        },
        {
            name: 'Je-ron',
            nicknames: [
                'jeroenboonekamp',
                'jboonekamp',
                'roeiert',
                'jeron'
            ]
        },
        {
            name: 'Bask',
            nicknames: [
                'baskerkhof',
                'bkerkhof',
                'opperjos'
            ]
        },
        {
            name: 'Bvdw',
            nicknames: [
                'basvanderwaal',
                'bvdw',
                'bvanderwaal',
                'beeveedeewee',
                'basvdwaal',
                'bvdwaal'
            ]
        },
        {
            name: 'Rens',
            nicknames: [
                'rensgeerling',
                'rgeerling',
                'rensrubengeerling'
            ]
        },
        {
            name: 'Marloes',
            nicknames: [
                'marloeskerkhof',
                'moeskerkhof'
            ]
        },
        {
            name: 'Löbker',
            nicknames: [
                'ricklobker',
                'ricklöbker',
                'rickert',
                'rickert',
                'riklobker',
                'riklöbker'
            ]
        },
        {
            name: 'Yanus',
            nicknames: [
                'yanniekvallenduuk',
                'yanusvallenduuk',
                'yvallenduuk'
            ]
        },
        {
            name: 'Jurre',
            nicknames: [
                'jurrevanderlaan',
                'jurvanderlaan',
                'rre',
                'jvdlaan'
            ]
        },
    ]

    validate(c: FormControl) {
        if(!c.value) {
            return {
                leeg: {
                    valid: false
                }
            }
        }

        if(c.value.length < 3) {
            return {
                kort: {
                    valid: false
                }
            }
        }

        const nameFull: string = c.value;
        const josnames = this.getNames(nameFull, this.josNames);

        if(josnames.length > 0) {
            return {
                jos: {
                    valid: false,
                }
            }
        }

        const names = this.getNames(nameFull, this.nicknames);
        if(names.length < 1) {
            return {
                name: {
                    valid: false
                }
            }
        }

        if(names.length > 1) {
            return {
                meer: {
                    valid: false
                }
            }
        }

        return null;

    }

    getNames(value: string, arr: {name: string, nicknames: string[]}[]) {
        const distinct = (v, i, s) => s.indexOf(v) === i;
        const validNicknames = [];
        const name = value.trim().replace(' ', '').replace('-', '').toLowerCase();
        arr.forEach(nick => nick.nicknames.forEach(n => n.includes(name) && validNicknames.push(nick.name)));
        return validNicknames.filter(distinct);
    }
   }