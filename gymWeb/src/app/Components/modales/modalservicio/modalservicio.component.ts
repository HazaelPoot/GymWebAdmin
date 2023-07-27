import { ImagepreviewService } from 'src/app/Services/imagepreview.service';
import { ServicioService } from 'src/app/Services/servicio.service';
import { SessionService } from 'src/app/Services/session.service';
import { Servicio } from 'src/app/Interfaces/servicio.interface';
import { LoaderService } from 'src/app/Services/loader.service';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-modalservicio',
  templateUrl: './modalservicio.component.html',
  styleUrls: ['./modalservicio.component.scss']
})
export class ModalservicioComponent implements OnInit {

  public idServicio: number = 0;
  public logoService: any = [];
  public add: boolean = true;
  public idGym: number;

  public servicioModel: Servicio = {
    idServicio: 0,
    nombre: '',
    costo: 0,
    detalles: '',
    nombreFoto: '',
    urlImagen: '',
    idGimnasio: 0,
    nombreGimnasio: ''
  }

  constructor(
    private previewService: ImagepreviewService,
    private servicioService: ServicioService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService,
    ) {
      this.idGym = this.sessionService.obtenerSession();
    }

  ngOnInit() {
   this.obtainIdServicio();
  }

  obtainIdServicio(): void {
    this.modalService.getValor().subscribe((valor: number) => {
      this.idServicio = valor;
      this.setModel();
    });
  }

  setModel(){
    this.servicioService.getById(this.idServicio).subscribe({
      next: (res) =>{
        this.resetearServicioModel();
        if(res.status == true){
          this.servicioModel = res.object;
          this.add = false;
        }
        else{
          this.add = true;
        }
      }
    });
  }

  //METODO POST (AGREGAR)
  btnGuardar(){
    if (this.servicioModel.nombre == '' || this.servicioModel.costo == 0 || this.servicioModel.detalles == '') {
      this._swal.swalValidator();
    }
    else{
      this.createServicio();
    }
  }

  createServicio(){
    this.loadingService.setLoading(true);
    const servicioModelModelo: string = JSON.stringify(this.servicioModel);
    const formData = new FormData();

    formData.append('modelo', servicioModelModelo);
    this.logoService.forEach((logoFoto: any) => {
      formData.append('foto', logoFoto);
    });

    this.servicioService.createServicio(this.idGym, formData).subscribe({
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

  //METODO PUT (ACTUALIZAR)
  btnActualizar(){
    if (this.servicioModel.nombre == '' || this.servicioModel.costo == 0 || this.servicioModel.detalles == '') {
      this._swal.swalValidator();
    }
    else{
      this.updateServicio();
    }
  }

  updateServicio(){
    this.loadingService.setLoading(true);
    const servicioModelModelo: string = JSON.stringify(this.servicioModel);
    const formData = new FormData();

    formData.append('modelo', servicioModelModelo);
    this.logoService.forEach((logoFoto: any) => {
      formData.append('foto', logoFoto);
    });

    this.servicioService.updateServicio(formData).subscribe({
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

  capturarPhoto(event: any){
    const fotoCapturada = event.target.files[0];
    this.previewService.base64Preview(fotoCapturada).then((imagen: any) =>{
      this.servicioModel.urlImagen  = imagen.base;
    })
    this.logoService.push(fotoCapturada);
    console.log(fotoCapturada);
  }

  resetearServicioModel(): void {
    this.servicioModel = {
      idServicio: 0,
      nombre: '',
      costo: 0,
      detalles: '',
      nombreFoto: '',
      urlImagen: 'assets/images/profile/NoImage.jpg',
      idGimnasio: 0,
      nombreGimnasio: ''
    };
  }
}