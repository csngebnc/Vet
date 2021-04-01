import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { AppointmentDto } from 'src/app/_models/appointmentdto';
import { VetUserDto } from 'src/app/_models/vetuserdto';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-doctor-homepage',
  templateUrl: './doctor-homepage.component.html',
  styleUrls: ['./doctor-homepage.component.css']
})
export class DoctorHomepageComponent implements OnInit {

  constructor(private userService: UserService, private appointmentService: AppointmentService) { }

  notesFormControl;

  doctor: VetUserDto;
  appointments: AppointmentDto[] = [];

  displayedColumns: string[] = ['startDate', 'treatmentName', 'doctorName', 'animalName', 'details'];
  dataSource;


  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((user: VetUserDto) => {
      this.doctor = user;
      let notes = localStorage.getItem('notes-' + this.doctor.id);
      this.notesFormControl = new FormControl('');
      this.notesFormControl.value = notes;
      this.appointmentService.doctorActiveAppointments(this.doctor.id).subscribe((appointments: AppointmentDto[]) => {
        this.appointments = appointments;
        this.dataSource = new MatTableDataSource<AppointmentDto>(this.appointments);
      })
    })
  }

  saveNotes() {
    if (this.notesFormControl.value.trim().length === 0) {
      localStorage.removeItem('notes-' + this.doctor.id)
    } else {
      localStorage.setItem('notes-' + this.doctor.id, this.notesFormControl.value);
    }
  }

  resignAppointment(id) {

  }

}
