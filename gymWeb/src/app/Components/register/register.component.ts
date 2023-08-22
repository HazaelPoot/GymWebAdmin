import { ImagepreviewService } from 'src/app/Services/imagepreview.service';
import { SessionService } from 'src/app/Services/session.service';
import { Gimnasio } from 'src/app/Interfaces/gimnasio.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { SwalService } from 'src/app/Services/swal.service';
import { GymService } from 'src/app/Services/gym.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  public previsualizacion: any = '/assets/images/logos/dark-logo.png';
  public confirmPass: string = '';
  public loading: boolean = false
  public logoGym: any = [];
  public idGym: number = 0;
  showPassword = false;

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
    private previewServie: ImagepreviewService,
    private loadingService: LoaderService,
    private sessionService: SessionService,
    private gymService: GymService,
    private swal: SwalService,
    private router: Router,
  ) { }

  ngOnInit(){
  }

  btnRegistrar(){
    console.log(this.gymModel)
    if (this.gymModel.passw != this.confirmPass){
      this.swal.swalPassword();
    }
    else if(this.verificarCamposVacios()){
      Swal.fire({
        icon: 'question',
        title: 'Confirmación',
        text: '¿Todos sus datos son correctos?',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No'
      }).then((result) => {
        if (result.isConfirmed) {
          this.registerGym();
        }
      });
    }
  }

  registerGym(){
    this.loadingService.setLoading(true);
    const gymModelModelo: string = JSON.stringify(this.gymModel);
    const formData = new FormData();

    formData.append('modelo', gymModelModelo);
    this.logoGym.forEach((logoFoto: any) => {
      formData.append('foto', logoFoto);
    });

    this.gymService.registerGym(formData).subscribe({
      next: (res) => {
        if(res.status == true){
          this.swal.swalRegister(res.message);
          this.idGym = res.object.idGimnasio;
          this.sessionService.guardarSession(this.idGym);
          this.router.navigate(['/Pages/Dashboard']);
        }
        else if(res.status == false){
          this.swal.swalOops(res.message);
        }
      },
      error: () => {
        this.swal.swalError();
      }
    });
  }

  capitalizarLetra(cadena: string) {
    return cadena.charAt(0).toUpperCase() + cadena.slice(1);
  }

  verificarCamposVacios() {
    const campos = Object.keys(this.gymModel) as (keyof Gimnasio)[];
    for (const campo of campos) {
      if (campo !== 'urlImagen' && campo !== 'nombreFoto' && this.gymModel[campo] === '') {
        const campoVacio = this.capitalizarLetra(campo);
        this.swal.swalValidatorMessage(campoVacio);
        return false;
      }
    }
    return true;
  }

  capturarPhoto(event: any){
    const fotoCapturada = event.target.files[0];
    this.previewServie.base64Preview(fotoCapturada).then((imagen: any) =>{
      this.previsualizacion = imagen.base;
    })
    this.logoGym.push(fotoCapturada);
  }

  btnShowPassword() {
    this.showPassword = !this.showPassword;
  }
}