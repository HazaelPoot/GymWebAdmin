import { SuscripcionService } from 'src/app/Services/suscripcion.service';
import { Susucripcion } from 'src/app/Interfaces/suscripcion.interface';
import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrls: ['./subscriptions.component.scss']
})
export class SubscriptionsComponent implements OnInit {

  suscripciones: Susucripcion[] = [];
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
    private suscService: SuscripcionService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.obtenerSuscripciones();
  }

  obtenerSuscripciones(){
    this.loadingService.setLoading(true);
    this.suscService.listSuscription(this.idGym).subscribe({
      next: (res) => {
        if (res.status == true) {
          this.suscripciones = res.listObject;
          this.loadingService.setLoading(false);
        } else {
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      },
    });
  }

  btnCambiarPago(item: Susucripcion) {
    this.suscripModel = item;
    Swal.fire({
      icon: 'question',
      title: 'Confirmación',
      text: '¿Estás seguro de cambiar el pago de la suscripción?',
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.cambiarPago();
      }
    });
  }

  cambiarPago(){
    this.loadingService.setLoading(true);
    this.suscService.changePay(this.suscripModel).subscribe({
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

  btnDelete(idsuscripcion: number) {
    Swal.fire({
      icon: 'question',
      title: 'Confirmación',
      text: `¿Estás seguro de eliminar esta suscripción?`,
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteSuscripcion(idsuscripcion);
      }
    });
  }

  deleteSuscripcion(idsuscripcion: number) {
    this.loadingService.setLoading(true);
    this.suscService.deleteSuscription(idsuscripcion).subscribe({
      next: (res) => {
        if (res.status == true) {
          this._swal.swalSucces(res.message);
        } else if (res.status == false) {
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      },
    });
  }
}