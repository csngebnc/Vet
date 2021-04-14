import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserPhotoComponent } from 'src/app/user/user-photo/user-photo.component';
import { VetUserDto } from 'src/app/_models/vetuserdto';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-user-homepage',
  templateUrl: './user-homepage.component.html',
  styleUrls: ['./user-homepage.component.css']
})
export class UserHomepageComponent implements OnInit {

  constructor(private userService: UserService, private modalService: NgbModal) { }

  user: VetUserDto;
  notesFormControl;

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((user: VetUserDto) => {
      this.user = user;
      let notes = localStorage.getItem('notes-' + this.user.id);
      this.notesFormControl = new FormControl('');
      this.notesFormControl.value = notes;
    })
  }

  openEdit() {
    const modalRef = this.modalService.open(UserPhotoComponent, { size: 'lg' });
    modalRef.componentInstance.userId = this.user.id;
    modalRef.componentInstance.photoPath = this.user.photoPath ? this.user.photoPath : "Images\\Users\\default.jpg";
    modalRef.result.then(() => {
      this.userService.getCurrentUser().subscribe((user: VetUserDto) => {
        this.user.photoPath = user.photoPath;
      })
    }, () => { })
  }

  deletePhoto() {
    this.userService.deletePhoto().subscribe((res: boolean) => {
      if (res) {
        this.user.photoPath = null;
      }
    });
  }

  saveNotes() {
    if (this.notesFormControl.value.trim().length === 0) {
      localStorage.removeItem('notes-' + this.user.id)
    } else {
      localStorage.setItem('notes-' + this.user.id, this.notesFormControl.value);
    }
  }

}
