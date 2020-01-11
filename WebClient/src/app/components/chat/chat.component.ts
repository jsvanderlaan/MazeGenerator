import { Component, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from "@angular/core";
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { LoginService } from 'src/app/services/login.service';
import { ChatMessage } from 'src/app/models/chat-message.model';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-chat',
    templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit{
    @ViewChild('chatWrapper', {static: false}) chatWrapper: ElementRef;
    private hubConnection: HubConnection;
    username;
    message = '';

    messages: ChatMessage[] = [];

    constructor(private readonly _loginService: LoginService, private readonly _cd: ChangeDetectorRef) { }

    ngOnInit() {
        this._loginService.getUsername$().subscribe(username => this.username = username);

        const prefix: string = environment.production ? '' : 'http://localhost:50485';
        this.hubConnection = new HubConnectionBuilder().withUrl(`${prefix}/chat`).build();
        this.hubConnection.start().then(() => {
            this.hubConnection.invoke('initChat');
        });

        this.hubConnection.on('messageQueue', messages => messages.forEach(this.addMessage));

        this.hubConnection.on('sendToAll', message => this.addMessage(message));
    }

    private addMessage = (message: ChatMessage) => {
        this.messages.push(message);
        this.messages.sort((a, b) => new Date(a.dateTime).getTime() - new Date(b.dateTime).getTime())
        this._cd.detectChanges();
        this.scrollDown();
    }

    private scrollDown = () => {
        if(this.chatWrapper){
            this.chatWrapper.nativeElement.scrollTop = this.chatWrapper.nativeElement.scrollHeight;
        }
    }

    public sendMessage(): void {
        this.hubConnection
          .invoke('sendToAll', this.username, this.message)
          .catch(err => console.error(err));
        this.message = '';
      }
}