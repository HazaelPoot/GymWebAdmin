import { SessionService } from 'src/app/Services/session.service';
import { Password } from 'src/app/Interfaces/password.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { SwalService } from 'src/app/Services/swal.service';
import { GymService } from 'src/app/Services/gym.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-deleteaccount',
  templateUrl: './deleteaccount.component.html',
  styleUrls: ['./deleteaccount.component.scss']
})
export class DeleteaccountComponent implements OnInit {

  public confirmClave: string = '';
  showPassConf = false;
  public idGym: number;

  public passModel: Password = {
    claveActual: '',
    claveNueva: ''
  }

  constructor(
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private gymService: GymService,
    private _swal: SwalService,
  ) {
    this.idGym = this.sessionService.obtenerSession()
  }

  ngOnInit(){
  }

  btnEliminar(){
    if(this.passModel.claveActual == '' || this.confirmClave == ''){
      this._swal.swalValidator();
    }
    else if(this.passModel.claveActual != this.confirmClave){
      this._swal.swalPassword();
    }
    else{
      Swal.fire({
        icon: 'question',
        title: 'Atención!',
        text: '¿Estás seguro que desea eliminar esta cuenta?',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No'
      }).then((result) => {
        if (result.isConfirmed) {
          this.eliminarCuenta();
        }
      });
    }
  }

  eliminarCuenta(){
    this.loadingService.setLoading(true);

    this.gymService.deleteAccount(this.idGym, this.passModel).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalDeleteAccount(res.message);
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

  btnPassConfirm() {
    this.showPassConf = !this.showPassConf;
  }
}