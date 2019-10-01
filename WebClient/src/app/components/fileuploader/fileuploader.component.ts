import { Component, OnInit } from '@angular/core';
import { FileUploadService } from 'src/app/services/file-upload.service';

@Component({
  selector: 'app-fileuploader',
  templateUrl: './fileuploader.component.html',
  styleUrls: ['./fileuploader.component.css']
})
export class FileuploaderComponent implements OnInit {

  maxFileSize = 1024 * 1024 * 10;
  allowedFileTypes = [
    "image/png",
    "image/jpg",
    "image/jpeg"
  ]

  file: File = null;
  dataUrl: string;
  loading = false;
  errors: string[] = [];
  constructor(private fileUploadService: FileUploadService) { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.file = files.item(0);
    if(this.file.size > this.maxFileSize) {
      this.errors.push("Upload a smaller file (max. 10MB)")  
    }
    if(!this.allowedFileTypes.includes(this.file.type)){
      this.errors.push("Upload a file with type jpeg, jpg or png")
    }
  }

  uploadFileToActivity() {
    this.loading = true;
    this.fileUploadService.postFile(this.file).subscribe(data => {
        this.dataUrl = `data:image/png;base64,${ data }`;
        this.loading = false;
      }, error => {
        this.loading = false;
      });
  }
}
