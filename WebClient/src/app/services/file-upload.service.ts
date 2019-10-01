import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class FileUploadService {

  constructor(
      private http: HttpClient
    ) { }

    postFile(fileToUpload: File): Observable<string> {
      const prefix = environment.production ? '' : 'http://localhost:50485'
        const endpoint =  prefix + '/api/fileupload';
        const formData: FormData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        return this.http.post<string>(endpoint, formData);
    }
}