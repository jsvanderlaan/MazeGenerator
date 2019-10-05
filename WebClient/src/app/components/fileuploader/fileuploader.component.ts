import { Component, Output, EventEmitter } from '@angular/core';
import { FileUploadService } from 'src/app/services/file-upload.service';

@Component({
  selector: 'app-fileuploader',
  templateUrl: './fileuploader.component.html',
  styleUrls: ['./fileuploader.component.css']
})
export class FileuploaderComponent {
  @Output() result = new EventEmitter<any>();
  constructor(private fileUploadService: FileUploadService) { }

  maxFileSize = 1024 * 1024 * 10;
  allowedFileTypes = [
    "image/png",
    "image/jpg",
    "image/jpeg"
  ]
  input: File = null;
  //dataUrl: string;
  loading = false;
  errors: string[] = [];

  handleFileInput(files: FileList) {
    this.errors = [];
    this.input = files.item(0);
    if(this.input.size > this.maxFileSize) {
      this.errors.push("Choose a smaller file (max. 10MB)")  
    }
    if(!this.allowedFileTypes.includes(this.input.type)){
      this.errors.push("Choose a file with type jpeg, jpg or png")
    }
  }

  removeFile(){
    this.input = null;
  }

  uploadFileToActivity() {
    this.loading = true;
    this.result.emit('assets/loading.gif');
    this.fileUploadService.postFile(this.input).subscribe(data => {
        this.result.emit(`data:image/png;base64,${ data }`);
        this.loading = false;
      }, error => {
        this.result.emit(null);
        this.loading = false;
      });
  }
}
