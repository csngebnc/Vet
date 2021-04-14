import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserPhotoComponent } from 'src/app/user/user-photo/user-photo.component';
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

  constructor(private userService: UserService, private appointmentService: AppointmentService, private modalService: NgbModal) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;

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

        this.dataSource.paginator = this.paginator;
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
    if (confirm('Biztosan le szeretnéd mondani az időpontot?')) {
      this.appointmentService.resignAppointmentByDoctor(id).subscribe((appointment: AppointmentDto) => {
        this.appointments = this.appointments.filter(a => a.id !== appointment.id);
        this.dataSource = new MatTableDataSource<AppointmentDto>(this.appointments);
        this.dataSource.paginator = this.paginator;
      })
    }
  }

  openEdit() {
    const modalRef = this.modalService.open(UserPhotoComponent, { size: 'lg' });
    modalRef.componentInstance.userId = this.doctor.id;
    modalRef.componentInstance.photoPath = this.doctor.photoPath ? this.doctor.photoPath : "Images\\Users\\default.jpg";
    modalRef.result.then(() => {
      this.userService.getCurrentUser().subscribe((user: VetUserDto) => {
        this.doctor.photoPath = user.photoPath;
      })
    }, () => { })
  }

  deletePhoto() {
    this.userService.deletePhoto().subscribe((res: boolean) => {
      if (res) {
        this.doctor.photoPath = null;
      }
    });
  }
}

