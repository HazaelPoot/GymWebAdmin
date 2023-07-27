import { ValidatormemberComponent } from './Pages/validatormember/validatormember.component';
import { DeleteaccountComponent } from './Pages/deleteaccount/deleteaccount.component';
import { SubscriptionsComponent } from './Pages/subscriptions/subscriptions.component'
import { GymservicesComponent } from './Pages/gymservices/gymservices.component';
import { InformationComponent } from './Pages/information/information.component';;
import { ChangepassComponent } from './Pages/changepass/changepass.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { SharedModule } from 'src/app/Reutilizable/shared/shared.module';
import { ScheduleComponent } from './Pages/schedule/schedule.component';
import { MembersComponent } from './Pages/members/members.component';
import { ModalesModule } from '../modales/modales.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [
    ChangepassComponent,
    DashboardComponent,
    DeleteaccountComponent,
    GymservicesComponent,
    InformationComponent,
    MembersComponent,
    ScheduleComponent,
    SubscriptionsComponent,
    ValidatormemberComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    //SHARED INCLUYE TODOS LOS MODULOS Y COMPONENTES GLOBALES
    SharedModule,
    ModalesModule
  ],
})
export class LayoutModule { }