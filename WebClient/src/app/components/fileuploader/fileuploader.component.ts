import { Component, OnInit } from '@angular/core';
import { FileUploadService } from 'src/app/services/file-upload.service';

@Component({
  selector: 'app-fileuploader',
  templateUrl: './fileuploader.component.html',
  styleUrls: ['./fileuploader.component.css']
})
export class FileuploaderComponent implements OnInit {

  file: File = null;
  constructor(private fileUploadService: FileUploadService) { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.file = files.item(0);
  }

  uploadFileToActivity() {
    this.fileUploadService.postFile(this.file).subscribe(data => {
        console.log(data);
      }, error => {
        console.log(error);
      });
  }
}
