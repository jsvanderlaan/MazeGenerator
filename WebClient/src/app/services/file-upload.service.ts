import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class FileUploadService {

  constructor(
      private http: HttpClient
    ) { }

    postFile(fileToUpload: File): Observable<Object> {
        const endpoint = '/api/fileupload';
        const formData: FormData = new FormData();
        formData.append('fileKey', fileToUpload, fileToUpload.name);
        return this.http.post(endpoint, formData);
    }
}