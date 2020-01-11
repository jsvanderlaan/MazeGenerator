import { Component, Output, EventEmitter } from "@angular/core";
import { StoryService } from 'src/app/services/story.service';
import { Story } from 'src/app/models/story.model';

@Component({
    selector: 'app-story-form',
    templateUrl: './story-form.component.html'})
export class StoryFormComponent {
    @Output() saved: EventEmitter<Story> = new EventEmitter<Story>();

    author = '';
    title = '';
    story = '';

    submitted = false;

    constructor(private storyService: StoryService) { }
    onSubmit = () => {
        this.submitted = true;
        const story = new Story(this.author, this.title, this.story, new Date());
        this.storyService.save(story).subscribe(() => this.saved.emit(story));
    }
} 