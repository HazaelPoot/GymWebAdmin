import { SessionService } from 'src/app/Services/session.service';
import { Password } from 'src/app/Interfaces/password.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { SwalService } from 'src/app/Services/swal.service';
import { GymService } from 'src/app/Services/gym.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-changepass',
  templateUrl: './changepass.component.html',
  styleUrls: ['./changepass.component.scss']
})
export class ChangepassComponent implements OnInit {

  public confirmClave: string = '';
  public idGym: number;
  showPassword = false;

  public passModel: Password = {
    claveActual: '',
    claveNueva: ''
  }

  constructor(
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private gymService: GymService,
    private swal: SwalService,
  ) {
    this.idGym = this.sessionService.obtenerSession()
  }

  ngOnInit(){
  }

  btnConfirmar(){
    if (Object.values(this.passModel).some(value => value === '')) {
      this.swal.swalValidator();
    }
    else if(this.passModel.claveNueva != this.confirmClave){
      this.swal.swalPassword();
    }
    else{
      Swal.fire({
        icon: 'question',
        title: 'Confirmación',
        text: '¿Estás seguro que desea cambiar su contraseña?',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No'
      }).then((result) => {
        if (result.isConfirmed) {
          this.cambiarClave();
        }
      });
    }
  }

  cambiarClave(){
    this.loadingService.setLoading(true);
    
    this.gymService.changePass(this.idGym, this.passModel).subscribe({
      next: (res) => {
        if(res.status == true){
          this.swal.swalSucces(res.message);
        }
        else{
          this.swal.swalOops(res.message );
        }
      },
      error: () => {
        this.swal.swalError();
      }
    });
  }

  btnShowPassword() {
    this.showPassword = !this.showPassword;
  }
}