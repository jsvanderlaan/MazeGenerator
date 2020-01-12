import { Directive } from '@angular/core';
import { Validator, FormControl, NG_VALIDATORS } from '@angular/forms';
import { NameService } from '../services/name.service';

@Directive({
    selector: '[validName][ngModel]',
    providers: [
      { provide: NG_VALIDATORS, useExisting: NameValidatorDirective, multi: true }
    ]
  })
  export class NameValidatorDirective implements Validator { 
    constructor(private _nameService: NameService){ }

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
        const josnames = this._nameService.getJosNames(nameFull);

        if(josnames.length > 0) {
            return {
                jos: {
                    valid: false,
                }
            }
        }

        const names = this._nameService.getNames(nameFull);
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
}