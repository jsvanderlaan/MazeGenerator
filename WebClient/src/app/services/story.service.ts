import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BaseService } from './base.service';
import { Story } from '../models/story.model';

@Injectable({
    providedIn: 'root'
})
export class StoryService {

    constructor(private base: BaseService) { }

    private endpoint = 'story';

    get = (): Observable<Story[]> => this.base.get(this.endpoint);

    save = (story: any) => this.base.post(this.endpoint, story);
}