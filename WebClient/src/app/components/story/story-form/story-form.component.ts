import { Component, Output, EventEmitter } from "@angular/core";
import { StoryService } from 'src/app/services/story.service';
import { Story } from 'src/app/models/story.model';
import { LoginService } from 'src/app/services/login.service';

@Component({
    selector: 'app-story-form',
    templateUrl: './story-form.component.html'})
export class StoryFormComponent {
    @Output() saved: EventEmitter<Story> = new EventEmitter<Story>();

    title = '';
    story = '';

    submitted = false;

    constructor(private storyService: StoryService, private readonly _loginService: LoginService) { }
    onSubmit = () => {
        this.submitted = true;
        this._loginService.getUsername$().subscribe(name => {
            const story = new Story(name, this.title, this.story, new Date());
            this.storyService.save(story).subscribe(() => this.saved.emit(story));
        })
    }
} 