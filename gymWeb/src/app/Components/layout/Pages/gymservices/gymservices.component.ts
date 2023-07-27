import { ServicioService } from 'src/app/Services/servicio.service';
import { SessionService } from 'src/app/Services/session.service';
import { Servicio } from 'src/app/Interfaces/servicio.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-gymservices',
  templateUrl: './gymservices.component.html',
  styleUrls: ['./gymservices.component.scss']
})
export class GymservicesComponent implements OnInit {

  servicios: Servicio[] = [];
  public idGym: number;

  constructor(
    private servicioService: ServicioService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService,
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.obtenerServicios();
  }

  obtenerServicios(){
    this.loadingService.setLoading(true);
    this.servicioService.listaServicio(this.idGym).subscribe({
      next: (res) => {
        if(res.status == true){
          this.servicios = res.listObject;
          this.loadingService.setLoading(false);
        }
        else{
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      }
    });
  }
  
  enviarValor(idServicio: number): void {
    this.modalService.enviarValor(idServicio);
  }

  btnDelete(idServicio: number){
    Swal.fire({
      icon: 'question',
      title: 'Confirmación',
      text: `¿Estás seguro de eliminar este serivicio?`,
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteService(idServicio);
      }
    });
  }

  deleteService(idServicio: number){
    this.loadingService.setLoading(true);
    this.servicioService.deleteServicio(idServicio).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalSucces(res.message);
        }
        else if(res.status == false){
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      }
    });
  }
}