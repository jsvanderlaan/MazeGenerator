import { Component } from "@angular/core";
import { StoryService } from 'src/app/services/story.service';
import { Story } from 'src/app/models/story.model';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-story',
    templateUrl: './story.component.html'})
export class StoryComponent {
    stories: Story[];
    constructor(private storyService: StoryService) {
        storyService.get().subscribe(stories => this.stories = stories);
    }
    storySaved(event) {
        this.stories.unshift(event);
    }
} 