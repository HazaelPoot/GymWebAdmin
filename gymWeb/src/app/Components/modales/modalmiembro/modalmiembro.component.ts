import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { MemberService } from 'src/app/Services/member.service';
import { Usuario } from 'src/app/Interfaces/usuario.interface';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-modalmiembro',
  templateUrl: './modalmiembro.component.html',
  styleUrls: ['./modalmiembro.component.scss']
})
export class ModalmiembroComponent implements OnInit {

  public confirmClave: string = '';
  showPassword = false;
  public idGym: number;

  public miembroModel: Usuario = {
    idUsuario: 0,
    nombre: '',
    apellido: '',
    edad: 0,
    contacto: '',
    correo: '',
    passw: '',
    fechaInscripcion: Date.now().toString(),
  }

  constructor(
    private memberService: MemberService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private _swal: SwalService,
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
  }

  btnGuardar(){
    console.log(this.miembroModel);
    if(Object.values(this.miembroModel).some(value => value === '') || this.miembroModel.edad == 0){
      this._swal.swalValidator();
    }
    else if(this.miembroModel.passw != this.confirmClave){
      this._swal.swalPassword();
    }
    else{
      this.agregarMiembro();
    }
  }

  agregarMiembro(){
    this.loadingService.setLoading(true);
    this.memberService.createMiembro(this.idGym, this.miembroModel).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalSucces(res.message);
          console.log(res);
        }else{
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      }
    });
  }

  btnShowPassword() {
    this.showPassword = !this.showPassword;
  }
}
