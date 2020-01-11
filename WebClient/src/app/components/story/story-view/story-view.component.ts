import { Component, Input } from "@angular/core";
import { Story } from 'src/app/models/story.model';

@Component({
    selector: 'app-story-view',
    templateUrl: './story-view.component.html'})
export class StoryViewComponent {
    @Input() story: Story;
} 