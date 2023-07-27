import { SessionService } from './Services/session.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'gymWeb';
  public idGym: number;

  constructor(
    private sessionService: SessionService,
    private router: Router
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }
  
  ngOnInit(){
    this.confirmAuth();
  }

  confirmAuth(){
    if(this.idGym == null){
      this.router.navigate(['/Login']);
    }
  }
}