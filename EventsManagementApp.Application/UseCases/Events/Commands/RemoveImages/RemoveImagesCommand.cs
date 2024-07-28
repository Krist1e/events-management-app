﻿using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public record RemoveImagesCommand(RemoveImagesRequest Images, string EventId) : IRequest<bool>;