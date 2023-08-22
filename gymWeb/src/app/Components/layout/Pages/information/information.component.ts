import { ImagepreviewService } from 'src/app/Services/imagepreview.service';
import { SessionService } from 'src/app/Services/session.service';
import { Gimnasio } from 'src/app/Interfaces/gimnasio.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { SwalService } from 'src/app/Services/swal.service';
import { GymService } from 'src/app/Services/gym.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss']
})
export class InformationComponent implements OnInit {

  
  public logoGym: any = [];
  public idGym: number;
  public nombreFoto: string = '';
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
    private previewService: ImagepreviewService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private gymService: GymService,
    private _swal: SwalService,
  ) { 
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.gymLogueado();
  }

  gymLogueado(){
    this.loadingService.setLoading(true);
    this.gymService.obtainGym(this.idGym).subscribe({
      next: (res) => {
        this.gymModel = res.object;
        this.nombreFoto = res.object.nombreFoto;
        this.loadingService.setLoading(false);
      }
    })
  }

  btnGuardar(){
    if (this.verificarCamposVacios()) {
      Swal.fire({
        icon: 'question',
        title: 'Confirmación',
        text: '¿Estás seguro de actualizar tu información?',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
      }).then((result) => {
        if (result.isConfirmed) {
          this.saveProfile();
        }
      });
    }
  }

  saveProfile(){
    this.loadingService.setLoading(true);
    const gymModelModelo: string = JSON.stringify(this.gymModel);
    const formData = new FormData();

    formData.append('modelo', gymModelModelo);
    this.logoGym.forEach((logoFoto: any) => {
      formData.append('foto', logoFoto);
    });

    this.gymService.guardarPerfil(formData).subscribe({
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
    })
  }

  capitalizarLetra(cadena: string) {
    return cadena.charAt(0).toUpperCase() + cadena.slice(1);
  }

  verificarCamposVacios() {
    const campos = Object.keys(this.gymModel) as (keyof Gimnasio)[];
    for (const campo of campos) {
      if (campo !== 'urlImagen' && campo !== 'nombreFoto' && this.gymModel[campo] === '') {
        const campoVacio = this.capitalizarLetra(campo);
        this._swal.swalValidatorMessage(campoVacio);
        return false;
      }
    }
    return true;
  }

  capturarPhoto(event: any){
    const fotoCapturada = event.target.files[0];
    this.previewService.base64Preview(fotoCapturada).then((imagen: any) =>{
      this.gymModel.urlImagen = imagen.base;
    })
    this.logoGym.push(fotoCapturada);
  }
}