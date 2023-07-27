import { SuscripcionService } from 'src/app/Services/suscripcion.service';
import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-validatormember',
  templateUrl: './validatormember.component.html',
  styleUrls: ['./validatormember.component.scss']
})
export class ValidatormemberComponent implements OnInit {

  public idGym: number;
  public nombre: string = '';
  
  constructor(
    private suscService: SuscripcionService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private _swal: SwalService
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
  }

  btnValidar(){
    if(this.nombre == ''){
      this._swal.swalValidator();
    }
    else{
      this.validarMiembro();
    }

  }
  validarMiembro(){
    this.loadingService.setLoading(true);
    this.suscService.searchMember(this.idGym, this.nombre).subscribe({
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
}