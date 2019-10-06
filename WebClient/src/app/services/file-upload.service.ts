import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class FileUploadService {

  constructor(
      private http: HttpClient
    ) { }

    private prefix = environment.production ? '' : 'http://localhost:50485';
    private endpoint =  this.prefix + '/api/fileupload';

    postFile(fileToUpload: File): Observable<string> {
      const formData: FormData = new FormData();
      formData.append('file', fileToUpload, fileToUpload.name);
      return this.http.post<string>(this.endpoint, formData);
    }
}