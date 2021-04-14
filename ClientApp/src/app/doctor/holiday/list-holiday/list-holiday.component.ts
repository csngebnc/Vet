import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HolidayDto } from 'src/app/_models/holidaydto';
import { DoctorService } from 'src/app/_services/doctor.service';
import { AddHolidayComponent } from '../add-holiday/add-holiday.component';
import { EditHolidayComponent } from '../edit-holiday/edit-holiday.component';

@Component({
  selector: 'app-list-holiday',
  templateUrl: './list-holiday.component.html',
  styleUrls: ['./list-holiday.component.css']
})
export class ListHolidayComponent implements OnInit {

  holidays: HolidayDto[] = [];

  dataSource;
  displayedColumns: string[] = ['firstDate', 'lastDate', 'button'];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private doctorService: DoctorService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.doctorService.getOwnHolidays().subscribe((holidays: HolidayDto[]) => {
      this.holidays = holidays;
      this.holidays.forEach(h => {
        h.startDate = new Date(h.startDate);
        h.endDate = new Date(h.endDate);
      });
      this.dataSource = new MatTableDataSource<HolidayDto>(this.holidays);
      this.dataSource.paginator = this.paginator;
    })
  }


  open() {
    const modalRef = this.modalService.open(AddHolidayComponent);
    modalRef.result.then((holiday: HolidayDto) => {
      holiday.startDate = new Date(holiday.startDate);
      holiday.endDate = new Date(holiday.endDate);
      this.holidays.push(holiday);
      this.dataSource = new MatTableDataSource<HolidayDto>(this.holidays);
    }, () => { })
  }

  openEdit(id) {
    const modalRef = this.modalService.open(EditHolidayComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then((holiday: HolidayDto) => {
      holiday.startDate = new Date(holiday.startDate);
      holiday.endDate = new Date(holiday.endDate);
      this.holidays[this.holidays.map(h => { return h.id }).indexOf(holiday.id)] = holiday;
      this.dataSource = new MatTableDataSource<HolidayDto>(this.holidays);
    }, () => { })
  }

  delete(id) {
    this.doctorService.deleteHoliday(id).subscribe(success => {
      if (success) {
        this.holidays = this.holidays.filter(h => h.id !== id);
        this.dataSource = new MatTableDataSource<HolidayDto>(this.holidays);
        this.dataSource.paginator = this.paginator;
      }
    })
  }



}
