import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FileuploaderComponent } from './components/maze-generator/fileuploader/fileuploader.component';

import { FileUploadService } from './services/file-upload.service';
import { HttpClientModule } from '@angular/common/http';
import { ResultDisplayComponent } from './components/maze-generator/result-display/result-display.component';
import { HeaderComponent } from './components/header/header.component';
import { FileSizePipe } from './pipes/file-size.pipe';
import { CountService } from './services/count.service';
import { BaseService } from './services/base.service';
import { ExerciseGeneratorComponent } from './components/exercise-generator/exercise-generator.component';
import { StoryComponent } from './components/story/story.component';
import { StoryFormComponent } from './components/story/story-form/story-form.component';
import { StoryViewComponent } from './components/story/story-view/story-view.component';
import { NgbTabsetModule } from '@ng-bootstrap/ng-bootstrap';
import { ChatComponent } from './components/chat/chat.component';
import { LoginComponent } from './components/login/login.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {MatIconModule} from '@angular/material/icon';
import { ChatDatePipe } from './pipes/chat-date.pipe';

@NgModule({
  declarations: [
    AppComponent,
    FileuploaderComponent,
    ResultDisplayComponent,
    HeaderComponent,
    ExerciseGeneratorComponent,
    StoryComponent,
    StoryFormComponent,
    StoryViewComponent,
    LoginComponent,
    ChatComponent,
    FileSizePipe,
    ChatDatePipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbTabsetModule,
    NoopAnimationsModule,
    MatIconModule
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
