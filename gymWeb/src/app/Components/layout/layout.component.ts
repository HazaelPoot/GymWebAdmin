import { SessionService } from 'src/app/Services/session.service';
import { Gimnasio } from 'src/app/Interfaces/gimnasio.interface';
import { GymService } from 'src/app/Services/gym.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  public idGym: number;
  public gymModel: Gimnasio = {
    idGimnasio: 0,
    nombre: '',
    contacto: '',
    direccion: '',
    ciudad: '',
    correo: '',
    passw: '',
    urlImagen: '',
    nombreFoto: '',
  };
  
  constructor(
    private sessionService: SessionService,
    private gymService: GymService,
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.gymLogueado();
  }

  gymLogueado(){
    this.gymService.obtainGym(this.idGym).subscribe({
      next: (res) => {
        this.gymModel = res.object;
      }
    })
  }

  logOut(){
    Swal.fire({
      icon: 'question',
      title: 'Salir',
      text: '¿Está seguro que desea cerrar la sesión?',
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.sessionService.eliminarSession();
        location.reload();
      }
    });
  }
}