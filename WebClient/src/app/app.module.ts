import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FileuploaderComponent } from './components/fileuploader/fileuploader.component';

import { FileUploadService } from './services/file-upload.service';
import { HttpClientModule } from '@angular/common/http';
import { ResultDisplayComponent } from './components/result-display/result-display.component';
import { HeaderComponent } from './components/header/header.component';
import { FileSizePipe } from './pipes/file-size.pipe';
import { CountService } from './services/count.service';
import { BaseService } from './services/base.service';

@NgModule({
  declarations: [
    AppComponent,
    FileuploaderComponent,
    ResultDisplayComponent,
    HeaderComponent,
    FileSizePipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    FileUploadService,
    CountService,
    BaseService,
    FileSizePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
