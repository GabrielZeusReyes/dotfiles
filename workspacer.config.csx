#r "C:\Users\Gabriel\scoop\apps\workspacer\current\workspacer.Shared.dll"
#r "C:\Users\Gabriel\scoop\apps\workspacer\current\plugins\workspacer.Bar\workspacer.Bar.dll"
#r "C:\Users\Gabriel\scoop\apps\workspacer\current\plugins\workspacer.ActionMenu\workspacer.ActionMenu.dll"
#r "C:\Users\Gabriel\scoop\apps\workspacer\current\plugins\workspacer.FocusIndicator\workspacer.FocusIndicator.dll"
#r "C:\Users\Gabriel\scoop\apps\workspacer\current\plugins\workspacer.Gap\workspacer.Gap.dll"

using System;
using System.Collections.Generic;
using System.Linq;
using workspacer;
using workspacer.Bar;
using workspacer.Bar.Widgets;
using workspacer.Gap;
//using workspacer.ActionMenu;
// using workspacer.FocusIndicator;

return new Action<IConfigContext>((IConfigContext context) =>
{
    /* Variables */
    /* Variables */
    var fontSize = 12;
    var barHeight = 19;
    var fontName = "JetBrainsMono NF";
    var background = new Color(0x0, 0x0, 0x0);
    var blue = new Color(0x89, 0xB4, 0xFA);

    /* Config */
    context.CanMinimizeWindows = true;

    /* Gap */
    var gap = barHeight - 8;
    var gapPlugin = context.AddGap(new GapPluginConfig() { InnerGap = gap, OuterGap = gap / 2, Delta = gap / 2 });

    /* Bar */
    context.AddBar(new BarPluginConfig()
    {
        FontSize = fontSize,
        BarHeight = barHeight,
        FontName = fontName,
        DefaultWidgetBackground = background,
        LeftWidgets = () => new IBarWidget[]
        {
            new WorkspaceWidget() {
              // WorkspaceHasFocusColor = blue,
            }, new TextWidget(" "), new TitleWidget() {
              IsShortTitle = true,
              MonitorHasFocusColor = new Color(0x02, 0xF0, 0xFF)
            }
        },
        RightWidgets = () => new IBarWidget[]
        {
            new BatteryWidget(),
            new TimeWidget(1000, "dd MMMM yyyy HH:mm tt"),
            // new ActiveLayoutWidget(),
        }
    });

    /* Bar focus indicator */
    // context.AddFocusIndicator(
    //     new FocusIndicatorPluginConfig(){
    //       BorderColor = background
    //     }
    // );

    /* Default layouts */
    Func<ILayoutEngine[]> defaultLayouts = () => new ILayoutEngine[]
    {
        new TallLayoutEngine(),
        new VertLayoutEngine(),
        new HorzLayoutEngine(),
        new FullLayoutEngine(),
    };

    context.DefaultLayouts = defaultLayouts;

    /* Workspaces */
    // Array of workspace names and their layouts
    (string, ILayoutEngine[])[] workspaces =
    {
        ("main", defaultLayouts()),
        ("full", defaultLayouts()),
    };

    foreach ((string name, ILayoutEngine[] layouts) in workspaces)
    {
      context.WorkspaceContainer.CreateWorkspace(name, layouts);
    }

    /* Action menu */
    // var actionMenu = context.AddActionMenu(new ActionMenuPluginConfig()
    // {
    //     RegisterKeybind = false,
    //     MenuHeight = barHeight,
    //     FontSize = fontSize,
    //     FontName = fontName,
    //     Background = background,
    // });

    // /* Action menu builder */
    // Func<ActionMenuItemBuilder> createActionMenuBuilder = () =>
    // {
    //     var menuBuilder = actionMenu.Create();

    //     // Switch to workspace
    //     menuBuilder.AddMenu("switch", () =>
    //     {
    //         var workspaceMenu = actionMenu.Create();
    //         var monitor = context.MonitorContainer.FocusedMonitor;
    //         var workspaces = context.WorkspaceContainer.GetWorkspaces(monitor);

    //         Func<int, Action> createChildMenu = (workspaceIndex) => () =>
    //         {
    //             context.Workspaces.SwitchMonitorToWorkspace(monitor.Index, workspaceIndex);
    //         };

    //         int workspaceIndex = 0;
    //         foreach (var workspace in workspaces)
    //         {
    //             workspaceMenu.Add(workspace.Name, createChildMenu(workspaceIndex));
    //             workspaceIndex++;
    //         }

    //         return workspaceMenu;
    //     });

    //     // Move window to workspace
    //     menuBuilder.AddMenu("move", () =>
    //     {
    //         var moveMenu = actionMenu.Create();
    //         var focusedWorkspace = context.Workspaces.FocusedWorkspace;

    //         var workspaces = context.WorkspaceContainer.GetWorkspaces(focusedWorkspace).ToArray();
    //         Func<int, Action> createChildMenu = (index) => () => { context.Workspaces.MoveFocusedWindowToWorkspace(index); };

    //         for (int i = 0; i < workspaces.Length; i++)
    //         {
    //             moveMenu.Add(workspaces[i].Name, createChildMenu(i));
    //         }

    //         return moveMenu;
    //     });

    //     // Rename workspace
    //     menuBuilder.AddFreeForm("rename", (name) =>
    //     {
    //         context.Workspaces.FocusedWorkspace.Name = name;
    //     });

    //     // Create workspace
    //     menuBuilder.AddFreeForm("create workspace", (name) =>
    //     {
    //         context.WorkspaceContainer.CreateWorkspace(name);
    //     });

    //     // Delete focused workspace
    //     menuBuilder.Add("close", () =>
    //     {
    //         context.WorkspaceContainer.RemoveWorkspace(context.Workspaces.FocusedWorkspace);
    //     });

    //     // Workspacer
    //     menuBuilder.Add("toggle keybind helper", () => context.Keybinds.ShowKeybindDialog());
    //     menuBuilder.Add("toggle enabled", () => context.Enabled = !context.Enabled);
    //     menuBuilder.Add("restart", () => context.Restart());
    //     menuBuilder.Add("quit", () => context.Quit());

    //     return menuBuilder;
    // };
    // var actionMenuBuilder = createActionMenuBuilder();

    /* Keybindings */
    Action setKeybindings = () =>
    {
        KeyModifiers winShift = KeyModifiers.Win | KeyModifiers.Shift;
        KeyModifiers winCtrl = KeyModifiers.Win | KeyModifiers.Control;
        KeyModifiers win = KeyModifiers.Win;
        KeyModifiers altRShift = KeyModifiers.RAlt | KeyModifiers.RShift;

        IKeybindManager manager = context.Keybinds;

        var workspaces = context.Workspaces;

        manager.UnsubscribeAll();
        // manager.Subscribe(MouseEvent.LButtonDown, () => workspaces.SwitchFocusedMonitorToMouseLocation());

        // // Left, Right keys
        // manager.Subscribe(winCtrl, Keys.Left, () => workspaces.SwitchToPreviousWorkspace(), "switch to previous workspace");
        // manager.Subscribe(winCtrl, Keys.Right, () => workspaces.SwitchToNextWorkspace(), "switch to next workspace");

        manager.Subscribe(winShift, Keys.Left, () => workspaces.MoveFocusedWindowToPreviousMonitor(), "move focused window to previous monitor");
        manager.Subscribe(winShift, Keys.Right, () => workspaces.MoveFocusedWindowToNextMonitor(), "move focused window to next monitor");

        // // H, L keys
        manager.Subscribe(winShift, Keys.H, () => workspaces.SwitchToPreviousWorkspace(), "switch to previous workspace");
        manager.Subscribe(winShift, Keys.L, () => workspaces.SwitchToNextWorkspace(), "switch to next workspace");

        manager.Subscribe(winCtrl, Keys.H, () => workspaces.FocusedWorkspace.ShrinkPrimaryArea(), "shrink primary area");
        manager.Subscribe(winCtrl, Keys.L, () => workspaces.FocusedWorkspace.ExpandPrimaryArea(), "expand primary area");


        // manager.Subscribe(winCtrl, Keys.H, () => workspaces.FocusedWorkspace.DecrementNumberOfPrimaryWindows(), "decrement number of primary windows");
        // manager.Subscribe(winCtrl, Keys.L, () => workspaces.FocusedWorkspace.IncrementNumberOfPrimaryWindows(), "increment number of primary windows");

        // // K, J keys
        manager.Subscribe(winShift, Keys.K, () => workspaces.FocusedWorkspace.FocusNextWindow(), "focus next window");
        manager.Subscribe(winShift, Keys.J, () => workspaces.FocusedWorkspace.FocusPreviousWindow(), "focus previous window");

        manager.Subscribe(winCtrl, Keys.K, () => workspaces.FocusedWorkspace.SwapFocusAndNextWindow(), "swap focus and next window");
        manager.Subscribe(winCtrl, Keys.J, () => workspaces.FocusedWorkspace.SwapFocusAndPreviousWindow(), "swap focus and previous window");

        // // Add, Subtract keys

        // context.Keybinds.Subscribe(win, Keys.B, () => System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"));
        // context.Keybinds.Subscribe(win, Keys.Enter, () => System.Diagnostics.Process.Start(@"C:\Users\iuseg\AppData\Local\Microsoft\WindowsApps\wt.exe"));

        manager.Subscribe(winCtrl, Keys.X, () => gapPlugin.IncrementInnerGap(), "increment inner gap");
        manager.Subscribe(winCtrl, Keys.Z, () => gapPlugin.DecrementInnerGap(), "decrement inner gap");

        manager.Subscribe(winShift, Keys.X, () => gapPlugin.IncrementOuterGap(), "increment outer gap");
        manager.Subscribe(winShift, Keys.Z, () => gapPlugin.DecrementOuterGap(), "decrement outer gap");

        // // Other shortcuts
        // manager.Subscribe(winCtrl, Keys.P, () => actionMenu.ShowMenu(actionMenuBuilder), "show menu");
        // manager.Subscribe(winShift, Keys.Escape, () => context.Enabled = !context.Enabled, "toggle enabled/disabled");
        // manager.Subscribe(winShift, Keys.I, () => context.ToggleConsoleWindow(), "toggle console window");
        
        manager.Subscribe(winShift, Keys.Q, () => context.Restart(), "restart workspacer");

        // Workspaces
        manager.Subscribe(altRShift, Keys.D1, () => context.Workspaces.SwitchToWorkspace(0), "switch to workspace 1");
        manager.Subscribe(altRShift, Keys.D2, () => context.Workspaces.SwitchToWorkspace(1), "switch to workspace 2");
    };
    setKeybindings();
});

/*
 * Sources
 * https://github.com/dalyIsaac/workspacer-config/blob/main/workspacer.config.csx
 * https://github.com/mushogenshin/workspacer_config/blob/main/workspacer.config.csx
 * https://github.com/BrotifyPacha/.workspacer/blob/master/workspacer.config.csx
 * */

