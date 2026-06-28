import { render, screen } from "@testing-library/angular";
import userEvent from "@testing-library/user-event";
import { GameFormComponent } from "./game-form";
import { Publisher } from "@core/models/game.model";

const publishers: Publisher[] = [
  { id: 1, name: "Nintendo", country: "Japan" },
  { id: 2, name: "Valve", country: "USA" },
];

describe("GameFormComponent", () => {
  it("does not emit save while required fields are empty", async () => {
    const save = jest.fn();
    await render(GameFormComponent, { inputs: { publishers }, on: { save } });

    await userEvent.click(screen.getByRole("button", { name: /create game/i }));
    expect(save).not.toHaveBeenCalled();
  });

  it("emits a typed payload when the form is valid", async () => {
    const save = jest.fn();
    await render(GameFormComponent, { inputs: { publishers }, on: { save } });

    await userEvent.type(screen.getByLabelText(/title/i), "Portal 2");
    await userEvent.type(screen.getByLabelText(/platform/i), "PC");
    await userEvent.clear(screen.getByLabelText(/price/i));
    await userEvent.type(screen.getByLabelText(/price/i), "14.99");
    await userEvent.type(screen.getByLabelText(/release date/i), "2011-04-19");
    await userEvent.selectOptions(screen.getByLabelText(/publisher/i), "Valve");

    await userEvent.click(screen.getByRole("button", { name: /create game/i }));

    expect(save).toHaveBeenCalledTimes(1);
    expect(save).toHaveBeenCalledWith(
      expect.objectContaining({
        title: "Portal 2",
        platform: "PC",
        publisherId: 2,
        price: 14.99,
      }),
    );
  });
});
