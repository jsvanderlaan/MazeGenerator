import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class CountService {

  constructor(
      private http: HttpClient
    ) { }

    private prefix = environment.production ? '' : 'http://localhost:50485';
    private endpoint =  this.prefix + '/api/count';

    countClick(name: string): Observable<any> {
      return this.http.get(this.endpoint + '/click/' + name);
    }
}