import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { MedicalRecordDto } from 'src/app/_models/MedicalRecordDto';
import { MedicalRecordService } from 'src/app/_services/medical-record.service';
import { EnlargeImageModalComponent } from '../enlarge-image-modal/enlarge-image-modal.component';

@Component({
  selector: 'app-list-all-records',
  templateUrl: './list-all-records.component.html',
  styleUrls: ['./list-all-records.component.css']
})
export class ListAllRecordsComponent implements OnInit {

  constructor(protected auth: AuthorizeService, private medicalRecordService: MedicalRecordService, private modalService: NgbModal) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;

  meds: MedicalRecordDto[] = [];
  selectedResult: MedicalRecordDto[] = [];
  pageSize = 10;
  length: number;
  pageSizeOptions: number[] = [10, 20, 50];
  pageEvent: PageEvent;

  ngOnInit(): void {
    this.medicalRecordService.getMyMedicalRecords().subscribe((meds: MedicalRecordDto[]) => {
      this.meds = meds;
      this.length = this.meds.length;
      this.selectedResult = this.meds.slice(0, this.pageSize);
    });
  }

  openBig(path) {
    const modalRef = this.modalService.open(EnlargeImageModalComponent, { size: 'lg' });
    modalRef.componentInstance.imagePath = path;
    modalRef.result.then(() => { }, () => { })
  }

  getData(event?: PageEvent) {
    this.selectedResult = this.meds.slice(event.pageIndex * event.pageSize, event.pageIndex * event.pageSize + event.pageSize);
    return event;
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }

  genPdf(id) {
    this.medicalRecordService.generatePdf(id).subscribe((response) => {
      var file = new Blob([response], { type: 'application/pdf' });
      var fileURL = URL.createObjectURL(file);
      window.open(fileURL);
    })
  }

}
