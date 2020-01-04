import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class BaseService {

    private prefix: string = environment.production ? '' : 'http://localhost:50485';
    private baseEndpoint: string = this.prefix + '/api/';
    constructor(private http: HttpClient) { }
    
    post(endpoint: string, data: any): Observable<any> {
      return this.http.post<any>(this.baseEndpoint + endpoint, data);
    }

    get = (endpoint: string) => this.http.get<any>(this.baseEndpoint + endpoint);
}