import { RegisterComponent } from './Components/register/register.component';
import { LayoutComponent } from './Components/layout/layout.component';
import { LoginComponent } from './Components/login/login.component';
import { SharedModule } from './Reutilizable/shared/shared.module';
import { LayoutModule } from './Components/layout/layout.module';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LayoutComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    //SE IMPORTAN LOS DOS PORQUE LAYOUT NO PUEDE ESTAR DENTRO DE SHARED POR LA RECURSIVIDAD.
    LayoutModule,
    SharedModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
