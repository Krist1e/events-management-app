﻿namespace EventsManagementApp.Application.UseCases.Events.Contracts;

public record RemoveImagesRequest(IEnumerable<string> ImageIds);