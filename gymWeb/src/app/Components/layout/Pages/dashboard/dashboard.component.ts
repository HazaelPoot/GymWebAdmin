import { SuscripcionService } from 'src/app/Services/suscripcion.service';
import { Susucripcion } from 'src/app/Interfaces/suscripcion.interface';
import { ScheduleService } from 'src/app/Services/schedule.service';
import { ServicioService } from 'src/app/Services/servicio.service';
import { SessionService } from 'src/app/Services/session.service';
import { Servicio } from 'src/app/Interfaces/servicio.interface';
import { MemberService } from 'src/app/Services/member.service';
import { Horario } from 'src/app/Interfaces/horario.interface';
import { Miembro } from 'src/app/Interfaces/miembro.interface';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  
  suscrips: Susucripcion[] = [];
  servicios: Servicio[] = [];
  horarios: Horario[] = [];
  miembros: Miembro[] = [];
  
  public idGym: number;

  constructor(
    private suscripService: SuscripcionService,
    private servicioService: ServicioService,
    private horarioService: ScheduleService,
    private sessionService: SessionService,
    private miembroService: MemberService,
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.obtenerContadores();
  }

  obtenerContadores(){
    this.servicioService.listaServicio(this.idGym).subscribe(res => {this.servicios = res.listObject});
    this.suscripService.listSuscription(this.idGym).subscribe(res => {this.suscrips = res.listObject});
    this.horarioService.listaHorario(this.idGym).subscribe(res => {this.horarios = res.listObject});
    this.miembroService.listaMiembro(this.idGym).subscribe(res => {this.miembros = res.listObject});
  }
}
