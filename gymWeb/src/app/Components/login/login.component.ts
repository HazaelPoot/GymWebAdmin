import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { LoginService } from 'src/app/Services/login.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Login } from 'src/app/Interfaces/login.interface';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginModel: Login = {
    correo: '',
    passw: ''
  }

  public idGym: number = 0;
  showPassword = false;

  constructor(
    private loginService: LoginService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private router: Router,
    private swal: SwalService
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.confirmAuth();
  }

  confirmAuth(){
    if(this.idGym != null){
      this.router.navigate(['/Pages/Dashboard']);
    }
  }

  login(){
    this.loadingService.setLoading(true);
    if(this.loginModel.correo == '' || this.loginModel.passw == ''){
      this.swal.swalValidator();
      this.loadingService.setLoading(false);
    }
    else{
      this.loginService.login(this.loginModel).subscribe({
        next: (res) => {
          if(res.status == true){
            this.idGym = res.object.idGimnasio;
            this.sessionService.guardarSession(this.idGym);
            this.router.navigate(['/Pages/Dashboard']);
          }
          else{
            this.swal.swalOops(res.message);
          }
        },
        error: () => {
          this.swal.swalError()
        }
      })
    }
  }

  btnShowPassword() {
    this.showPassword = !this.showPassword;
  }
}