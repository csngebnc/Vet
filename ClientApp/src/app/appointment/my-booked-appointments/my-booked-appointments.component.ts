import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AppointmentDto } from 'src/app/_models/appointmentdto';
import { AppointmentService } from 'src/app/_services/appointment.service';

@Component({
  selector: 'app-my-booked-appointments',
  templateUrl: './my-booked-appointments.component.html',
  styleUrls: ['./my-booked-appointments.component.css']
})
export class MyBookedAppointmentsComponent implements OnInit {
  my_appointments: AppointmentDto[] = [];
  my_old_appointments: AppointmentDto[] = [];
  constructor(private appointmentService: AppointmentService) { }

  displayedColumns: string[] = ['startDate', 'treatmentName', 'doctorName', 'animalName', 'details'];
  dataSource;
  dataSourceCurrent;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngOnInit(): void {
    this.appointmentService.getMyAppointments().subscribe((appointments: AppointmentDto[]) => {
      this.my_appointments = appointments;
      var now = new Date(Date.now())
      this.my_appointments.forEach(a => {
        a.startDate = new Date(a.startDate);
        a.endDate = new Date(a.endDate);
      })
      this.my_old_appointments = this.my_appointments.filter(a => a.endDate < now || a.isResigned);
      this.my_appointments = this.my_appointments.filter(a => a.startDate >= now && !a.isResigned);
      this.dataSource = new MatTableDataSource<AppointmentDto>(this.my_old_appointments);
      this.dataSourceCurrent = new MatTableDataSource<AppointmentDto>(this.my_appointments);

      this.dataSource.paginator = this.paginator;
    });
  }

  resignAppointment(id) {
    if (confirm("Biztosan le szeretnéd mondani ezt az időpontot?")) {
      this.appointmentService.resignAppointment(id).subscribe((appointment: AppointmentDto) => {
        appointment.startDate = new Date(appointment.startDate);
        appointment.endDate = new Date(appointment.endDate);

        this.my_appointments = this.my_appointments.filter(a => a.id !== appointment.id);
        this.my_old_appointments.push(appointment);
        this.my_old_appointments.sort((a, b) => (a.startDate > b.startDate) ? -1 : ((b.startDate > a.startDate) ? 1 :
          (a.id > b.id) ? -1 : (b.id > a.id) ? 1 : 0))

        this.dataSource = new MatTableDataSource<AppointmentDto>(this.my_old_appointments);
        this.dataSourceCurrent = new MatTableDataSource<AppointmentDto>(this.my_appointments);
      });
    }
  }

}
