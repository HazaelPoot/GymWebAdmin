import { SuscripcionService } from 'src/app/Services/suscripcion.service';
import { Susucripcion } from 'src/app/Interfaces/suscripcion.interface';
import { ServicioService } from 'src/app/Services/servicio.service';
import { SessionService } from 'src/app/Services/session.service';
import { Servicio } from 'src/app/Interfaces/servicio.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { MemberService } from 'src/app/Services/member.service';
import { Miembro } from 'src/app/Interfaces/miembro.interface';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-modalsuscripcion',
  templateUrl: './modalsuscripcion.component.html',
  styleUrls: ['./modalsuscripcion.component.scss']
})
export class ModalsuscripcionComponent implements OnInit {

  miembros: Miembro[] = [];
  servicios: Servicio[] = [];
  public idGym: number;
  
  public suscripModel: Susucripcion = {
    idInscripcion: 0,
    idMiembro: 0,
    nombre: '',
    apellido: '',
    idServicio: 0,
    nombreServicio: '',
    claveAcceso: '',
    fechaPago: '',
    montoPagar: 0,
    pago: 0,
    clasePago: ''
  }
  
  constructor(
    private readonly miembroService: MemberService,
    private readonly servicioService:  ServicioService,

    private suscriptionService: SuscripcionService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService,
    ) {
      this.idGym = this.sessionService.obtenerSession();
    }

  ngOnInit(){
    this.listMimebros();
    this.listServicios();
  }

  btnAgregar(){
    if(this.suscripModel.idMiembro == 0 || this.suscripModel.idServicio == 0){
      this._swal.swalValidator();
    }else{
      this.agregarSuscripcion();
    }
    console.log(this.suscripModel);
  }

  agregarSuscripcion(){
    this.loadingService.setLoading(true);
    this.suscriptionService.registerSuscription(this.suscripModel).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalSucces(res.message);
        }else{
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      }
    });
  }

  listMimebros(){
    this.miembroService.listaMiembro(this.idGym).subscribe({
      next: (res) => {
        this.miembros = res.listObject;
      }
    });
  }

  listServicios(){
    this.servicioService.listaServicio(this.idGym).subscribe({
      next: (res) => {
        this.servicios = res.listObject;
      }
    });
  }
}
