import { SessionService } from './session.service';
import { LoaderService } from './loader.service';
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SwalService {

  constructor(
    private sessionService: SessionService,
    private loadingService: LoaderService,
  ) { }

  swalSucces(message: string){
    this.loadingService.setLoading(false);
    Swal.fire({
      icon: 'success',
      title: 'Exito',
      text: message
    }).then((result) => {
      if (result.isConfirmed) {
        location.reload();
      }
    });
  }

  swalRegister(message: string){
    this.loadingService.setLoading(false);
    Swal.fire({
      icon: 'success',
      title: 'Exito',
      text: message
    });
  }
  
  swalDeleteAccount(message: string){
    this.loadingService.setLoading(false);
    Swal.fire({
      icon: 'success',
      title: 'Exito',
      text: message
    }).then((result) => {
      if (result.isConfirmed) {
        this.sessionService.eliminarSession();
        location.reload();
      }
    });
  }

  swalError(){
    this.loadingService.setLoading(false);
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: 'Tenemos problemas con el servidor, refresca la página o vuelve mas tarde.'
    });
  }

  swalOops(message: string){
    this.loadingService.setLoading(false);
    Swal.fire({
      icon: 'error',
      title: 'Oops...',
      text: message
    });
  }

  swalValidator(){
    Swal.fire({
      icon: 'warning',
      title: 'Atención',
      text: 'Complete todos los datos'
    });
  }

  swalValidatorMessage(text: string){
    Swal.fire({
      icon: 'warning',
      title: 'Atención',
      text: `Debe completar el campo ${text}`
    });
  }

  swalPassword(){
    Swal.fire({
      icon: 'warning',
      title: 'Atención',
      text: 'Las contraseñas no coinciden'
    });
  }
}