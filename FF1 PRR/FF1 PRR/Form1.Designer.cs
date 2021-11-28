﻿
namespace FF1_PRR
{
	partial class FF1PRR
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnRandomize = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.FF1PRFolder = new System.Windows.Forms.TextBox();
			this.CuteHats = new System.Windows.Forms.CheckBox();
			this.ShuffleBossSpots = new System.Windows.Forms.CheckBox();
			this.KeyItems = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.RandoFlags = new System.Windows.Forms.TextBox();
			this.NewChecksum = new System.Windows.Forms.Label();
			this.RandoSeed = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.NewSeed = new System.Windows.Forms.Button();
			this.VisualFlags = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.BrowseForFolder = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.RandoShop = new System.Windows.Forms.ComboBox();
			this.Traditional = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.randoMagic = new System.Windows.Forms.CheckBox();
			this.keepMagicPermissions = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.monsterXPGPBoost = new System.Windows.Forms.ComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label8 = new System.Windows.Forms.Label();
			this.flagT = new System.Windows.Forms.ComboBox();
			this.flagTraditionalTreasure = new System.Windows.Forms.CheckBox();
			this.flagRebalancePrices = new System.Windows.Forms.CheckBox();
			this.flagRestoreCritRating = new System.Windows.Forms.CheckBox();
			this.flagWandsAddInt = new System.Windows.Forms.CheckBox();
			this.flagFiendsDropRibbons = new System.Windows.Forms.CheckBox();
			this.flagRebalanceBosses = new System.Windows.Forms.CheckBox();
			this.btnRestoreVanilla = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRandomize
			// 
			this.btnRandomize.Location = new System.Drawing.Point(649, 541);
			this.btnRandomize.Margin = new System.Windows.Forms.Padding(2);
			this.btnRandomize.Name = "btnRandomize";
			this.btnRandomize.Padding = new System.Windows.Forms.Padding(3);
			this.btnRandomize.Size = new System.Drawing.Size(120, 33);
			this.btnRandomize.TabIndex = 0;
			this.btnRandomize.Text = "Randomize!";
			this.btnRandomize.UseVisualStyleBackColor = true;
			this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Margin = new System.Windows.Forms.Padding(2);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(3);
			this.label1.Size = new System.Drawing.Size(104, 26);
			this.label1.TabIndex = 1;
			this.label1.Text = "FF1 PR Folder";
			// 
			// FF1PRFolder
			// 
			this.FF1PRFolder.Location = new System.Drawing.Point(128, 14);
			this.FF1PRFolder.Margin = new System.Windows.Forms.Padding(2);
			this.FF1PRFolder.Name = "FF1PRFolder";
			this.FF1PRFolder.Size = new System.Drawing.Size(547, 27);
			this.FF1PRFolder.TabIndex = 2;
			// 
			// CuteHats
			// 
			this.CuteHats.AutoSize = true;
			this.CuteHats.Location = new System.Drawing.Point(485, 128);
			this.CuteHats.Margin = new System.Windows.Forms.Padding(2);
			this.CuteHats.Name = "CuteHats";
			this.CuteHats.Padding = new System.Windows.Forms.Padding(3);
			this.CuteHats.Size = new System.Drawing.Size(101, 30);
			this.CuteHats.TabIndex = 3;
			this.CuteHats.Text = "Cute Hats";
			this.toolTip1.SetToolTip(this.CuteHats, "Your hat is cute.");
			this.CuteHats.UseVisualStyleBackColor = true;
			this.CuteHats.Click += new System.EventHandler(this.DetermineFlags);
			// 
			// ShuffleBossSpots
			// 
			this.ShuffleBossSpots.AutoSize = true;
			this.ShuffleBossSpots.Enabled = false;
			this.ShuffleBossSpots.Location = new System.Drawing.Point(5, 22);
			this.ShuffleBossSpots.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.ShuffleBossSpots.Name = "ShuffleBossSpots";
			this.ShuffleBossSpots.Padding = new System.Windows.Forms.Padding(3);
			this.ShuffleBossSpots.Size = new System.Drawing.Size(158, 30);
			this.ShuffleBossSpots.TabIndex = 4;
			this.ShuffleBossSpots.Text = "Shuffle Boss Spots";
			this.toolTip1.SetToolTip(this.ShuffleBossSpots, "Change which boss appears at which boss location.");
			this.ShuffleBossSpots.UseVisualStyleBackColor = true;
			this.ShuffleBossSpots.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// KeyItems
			// 
			this.KeyItems.AutoSize = true;
			this.KeyItems.Location = new System.Drawing.Point(12, 128);
			this.KeyItems.Margin = new System.Windows.Forms.Padding(2);
			this.KeyItems.Name = "KeyItems";
			this.KeyItems.Padding = new System.Windows.Forms.Padding(3);
			this.KeyItems.Size = new System.Drawing.Size(180, 30);
			this.KeyItems.TabIndex = 5;
			this.KeyItems.Text = "Randomize &Key Items";
			this.toolTip1.SetToolTip(this.KeyItems, "Change which key item appears at each location. All key items are guaranteed to b" +
        "e accessible.");
			this.KeyItems.UseVisualStyleBackColor = true;
			this.KeyItems.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 51);
			this.label2.Margin = new System.Windows.Forms.Padding(2);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(3);
			this.label2.Size = new System.Drawing.Size(120, 26);
			this.label2.TabIndex = 6;
			this.label2.Text = "Gameplay Flags";
			// 
			// RandoFlags
			// 
			this.RandoFlags.Location = new System.Drawing.Point(128, 52);
			this.RandoFlags.Margin = new System.Windows.Forms.Padding(2);
			this.RandoFlags.Name = "RandoFlags";
			this.RandoFlags.Size = new System.Drawing.Size(346, 27);
			this.RandoFlags.TabIndex = 7;
			// 
			// NewChecksum
			// 
			this.NewChecksum.AutoSize = true;
			this.NewChecksum.Location = new System.Drawing.Point(12, 545);
			this.NewChecksum.Margin = new System.Windows.Forms.Padding(2);
			this.NewChecksum.Name = "NewChecksum";
			this.NewChecksum.Padding = new System.Windows.Forms.Padding(3);
			this.NewChecksum.Size = new System.Drawing.Size(273, 26);
			this.NewChecksum.TabIndex = 8;
			this.NewChecksum.Text = "New Checksum:  (Not Randomized Yet)";
			// 
			// RandoSeed
			// 
			this.RandoSeed.Location = new System.Drawing.Point(542, 52);
			this.RandoSeed.Margin = new System.Windows.Forms.Padding(2);
			this.RandoSeed.Name = "RandoSeed";
			this.RandoSeed.Size = new System.Drawing.Size(154, 27);
			this.RandoSeed.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(485, 51);
			this.label3.Margin = new System.Windows.Forms.Padding(2);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(3);
			this.label3.Size = new System.Drawing.Size(48, 26);
			this.label3.TabIndex = 9;
			this.label3.Text = "Seed";
			// 
			// NewSeed
			// 
			this.NewSeed.Location = new System.Drawing.Point(710, 50);
			this.NewSeed.Margin = new System.Windows.Forms.Padding(2);
			this.NewSeed.Name = "NewSeed";
			this.NewSeed.Padding = new System.Windows.Forms.Padding(3);
			this.NewSeed.Size = new System.Drawing.Size(59, 33);
			this.NewSeed.TabIndex = 11;
			this.NewSeed.Text = "New";
			this.NewSeed.UseVisualStyleBackColor = true;
			this.NewSeed.Click += new System.EventHandler(this.NewSeed_Click);
			// 
			// VisualFlags
			// 
			this.VisualFlags.Location = new System.Drawing.Point(128, 91);
			this.VisualFlags.Margin = new System.Windows.Forms.Padding(2);
			this.VisualFlags.Name = "VisualFlags";
			this.VisualFlags.Size = new System.Drawing.Size(346, 27);
			this.VisualFlags.TabIndex = 13;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 89);
			this.label4.Margin = new System.Windows.Forms.Padding(2);
			this.label4.Name = "label4";
			this.label4.Padding = new System.Windows.Forms.Padding(3);
			this.label4.Size = new System.Drawing.Size(114, 26);
			this.label4.TabIndex = 12;
			this.label4.Text = "Cosmetic Flags";
			// 
			// BrowseForFolder
			// 
			this.BrowseForFolder.Location = new System.Drawing.Point(686, 12);
			this.BrowseForFolder.Margin = new System.Windows.Forms.Padding(2);
			this.BrowseForFolder.Name = "BrowseForFolder";
			this.BrowseForFolder.Padding = new System.Windows.Forms.Padding(3);
			this.BrowseForFolder.Size = new System.Drawing.Size(82, 32);
			this.BrowseForFolder.TabIndex = 14;
			this.BrowseForFolder.Text = "Browse";
			this.BrowseForFolder.UseVisualStyleBackColor = true;
			this.BrowseForFolder.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 28);
			this.label5.Margin = new System.Windows.Forms.Padding(6);
			this.label5.Name = "label5";
			this.label5.Padding = new System.Windows.Forms.Padding(3);
			this.label5.Size = new System.Drawing.Size(58, 26);
			this.label5.TabIndex = 15;
			this.label5.Text = "&Shops:";
			this.toolTip1.SetToolTip(this.label5, "Randomize shop contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			// 
			// RandoShop
			// 
			this.RandoShop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RandoShop.FormattingEnabled = true;
			this.RandoShop.Items.AddRange(new object[] {
            "None",
            "Shuffle",
            "Standard",
            "Pro",
            "Wild"});
			this.RandoShop.Location = new System.Drawing.Point(121, 26);
			this.RandoShop.Margin = new System.Windows.Forms.Padding(6);
			this.RandoShop.Name = "RandoShop";
			this.RandoShop.Size = new System.Drawing.Size(151, 28);
			this.RandoShop.TabIndex = 16;
			this.toolTip1.SetToolTip(this.RandoShop, "Randomize shop contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			this.RandoShop.SelectedIndexChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// Traditional
			// 
			this.Traditional.AutoSize = true;
			this.Traditional.Location = new System.Drawing.Point(282, 24);
			this.Traditional.Margin = new System.Windows.Forms.Padding(6);
			this.Traditional.Name = "Traditional";
			this.Traditional.Padding = new System.Windows.Forms.Padding(3);
			this.Traditional.Size = new System.Drawing.Size(160, 30);
			this.Traditional.TabIndex = 17;
			this.Traditional.Text = "E&xclude DoS items";
			this.toolTip1.SetToolTip(this.Traditional, "Remove newer items (Ether, Phoenix Down, etc) from shops.");
			this.Traditional.UseVisualStyleBackColor = true;
			this.Traditional.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 262);
			this.label6.Margin = new System.Windows.Forms.Padding(2);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 20);
			this.label6.TabIndex = 18;
			// 
			// randoMagic
			// 
			this.randoMagic.AutoSize = true;
			this.randoMagic.Location = new System.Drawing.Point(8, 28);
			this.randoMagic.Margin = new System.Windows.Forms.Padding(6);
			this.randoMagic.Name = "randoMagic";
			this.randoMagic.Padding = new System.Windows.Forms.Padding(3);
			this.randoMagic.Size = new System.Drawing.Size(157, 30);
			this.randoMagic.TabIndex = 19;
			this.randoMagic.Text = "Randomize &Magic";
			this.toolTip1.SetToolTip(this.randoMagic, "Change which spells are available at each level, and in which shops they can be p" +
        "urchased.");
			this.randoMagic.UseVisualStyleBackColor = true;
			this.randoMagic.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// keepMagicPermissions
			// 
			this.keepMagicPermissions.AutoSize = true;
			this.keepMagicPermissions.Location = new System.Drawing.Point(172, 28);
			this.keepMagicPermissions.Margin = new System.Windows.Forms.Padding(6);
			this.keepMagicPermissions.Name = "keepMagicPermissions";
			this.keepMagicPermissions.Padding = new System.Windows.Forms.Padding(3);
			this.keepMagicPermissions.Size = new System.Drawing.Size(151, 30);
			this.keepMagicPermissions.TabIndex = 20;
			this.keepMagicPermissions.Text = "&Keep Permissions";
			this.toolTip1.SetToolTip(this.keepMagicPermissions, "Preserve who may learn each spell, rather than adjusting to the spell\'s new slot." +
        " (eg, level 1 Flare can still only be learned by Black Wizard)");
			this.keepMagicPermissions.UseVisualStyleBackColor = true;
			this.keepMagicPermissions.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(5, 28);
			this.label7.Name = "label7";
			this.label7.Padding = new System.Windows.Forms.Padding(3);
			this.label7.Size = new System.Drawing.Size(98, 26);
			this.label7.TabIndex = 21;
			this.label7.Text = "&XP/Gil Boost";
			this.toolTip1.SetToolTip(this.label7, "How much to increase earned XP and Gil.");
			// 
			// monsterXPGPBoost
			// 
			this.monsterXPGPBoost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.monsterXPGPBoost.FormattingEnabled = true;
			this.monsterXPGPBoost.Items.AddRange(new object[] {
            "0.5x",
            "1.0x",
            "1.5x",
            "2.0x",
            "3.0x",
            "4.0x",
            "5.0x",
            "10.0x"});
			this.monsterXPGPBoost.Location = new System.Drawing.Point(106, 26);
			this.monsterXPGPBoost.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.monsterXPGPBoost.Name = "monsterXPGPBoost";
			this.monsterXPGPBoost.Size = new System.Drawing.Size(51, 28);
			this.monsterXPGPBoost.TabIndex = 22;
			this.toolTip1.SetToolTip(this.monsterXPGPBoost, "How much to increase earned XP and Gil.");
			this.monsterXPGPBoost.SelectedIndexChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 67);
			this.label8.Margin = new System.Windows.Forms.Padding(6);
			this.label8.Name = "label8";
			this.label8.Padding = new System.Windows.Forms.Padding(3);
			this.label8.Size = new System.Drawing.Size(73, 26);
			this.label8.TabIndex = 18;
			this.label8.Text = "&Treasure:";
			this.toolTip1.SetToolTip(this.label8, "Randomize treasure chest contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			// 
			// flagT
			// 
			this.flagT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.flagT.FormattingEnabled = true;
			this.flagT.Items.AddRange(new object[] {
            "None",
            "Shuffle",
            "Standard",
            "Pro",
            "Wild"});
			this.flagT.Location = new System.Drawing.Point(121, 65);
			this.flagT.Margin = new System.Windows.Forms.Padding(6);
			this.flagT.Name = "flagT";
			this.flagT.Size = new System.Drawing.Size(151, 28);
			this.flagT.TabIndex = 19;
			this.toolTip1.SetToolTip(this.flagT, "Randomize treasure chest contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			this.flagT.SelectedIndexChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagTraditionalTreasure
			// 
			this.flagTraditionalTreasure.AutoSize = true;
			this.flagTraditionalTreasure.Enabled = false;
			this.flagTraditionalTreasure.Location = new System.Drawing.Point(282, 63);
			this.flagTraditionalTreasure.Margin = new System.Windows.Forms.Padding(6);
			this.flagTraditionalTreasure.Name = "flagTraditionalTreasure";
			this.flagTraditionalTreasure.Padding = new System.Windows.Forms.Padding(3);
			this.flagTraditionalTreasure.Size = new System.Drawing.Size(160, 30);
			this.flagTraditionalTreasure.TabIndex = 20;
			this.flagTraditionalTreasure.Text = "E&xclude DoS items";
			this.toolTip1.SetToolTip(this.flagTraditionalTreasure, "Remove newer items (Ether, Phoenix Down, etc) from shops.");
			this.flagTraditionalTreasure.UseVisualStyleBackColor = true;
			this.flagTraditionalTreasure.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagRebalancePrices
			// 
			this.flagRebalancePrices.AutoSize = true;
			this.flagRebalancePrices.Location = new System.Drawing.Point(5, 54);
			this.flagRebalancePrices.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.flagRebalancePrices.Name = "flagRebalancePrices";
			this.flagRebalancePrices.Padding = new System.Windows.Forms.Padding(3);
			this.flagRebalancePrices.Size = new System.Drawing.Size(183, 30);
			this.flagRebalancePrices.TabIndex = 21;
			this.flagRebalancePrices.Text = "Rebalance item prices";
			this.toolTip1.SetToolTip(this.flagRebalancePrices, "Adjust item prices for a more balanced experience. Increases price of HP/MP resto" +
        "rative items and Phoenix Down.");
			this.flagRebalancePrices.UseVisualStyleBackColor = true;
			this.flagRebalancePrices.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagRestoreCritRating
			// 
			this.flagRestoreCritRating.AutoSize = true;
			this.flagRestoreCritRating.Location = new System.Drawing.Point(5, 82);
			this.flagRestoreCritRating.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.flagRestoreCritRating.Name = "flagRestoreCritRating";
			this.flagRestoreCritRating.Padding = new System.Windows.Forms.Padding(3);
			this.flagRestoreCritRating.Size = new System.Drawing.Size(155, 30);
			this.flagRestoreCritRating.TabIndex = 22;
			this.flagRestoreCritRating.Text = "Restore crit rating";
			this.toolTip1.SetToolTip(this.flagRestoreCritRating, "Weapon critical rate changed to reflect original NES data.");
			this.flagRestoreCritRating.UseVisualStyleBackColor = true;
			this.flagRestoreCritRating.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagWandsAddInt
			// 
			this.flagWandsAddInt.AutoSize = true;
			this.flagWandsAddInt.Location = new System.Drawing.Point(5, 111);
			this.flagWandsAddInt.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.flagWandsAddInt.Name = "flagWandsAddInt";
			this.flagWandsAddInt.Padding = new System.Windows.Forms.Padding(3);
			this.flagWandsAddInt.Size = new System.Drawing.Size(138, 30);
			this.flagWandsAddInt.TabIndex = 23;
			this.flagWandsAddInt.Text = "Wands add INT";
			this.toolTip1.SetToolTip(this.flagWandsAddInt, "Staves and Hammers increase the user\'s Intelligence when equipped.");
			this.flagWandsAddInt.UseVisualStyleBackColor = true;
			this.flagWandsAddInt.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagFiendsDropRibbons
			// 
			this.flagFiendsDropRibbons.AutoSize = true;
			this.flagFiendsDropRibbons.Location = new System.Drawing.Point(5, 79);
			this.flagFiendsDropRibbons.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.flagFiendsDropRibbons.Name = "flagFiendsDropRibbons";
			this.flagFiendsDropRibbons.Padding = new System.Windows.Forms.Padding(3);
			this.flagFiendsDropRibbons.Size = new System.Drawing.Size(173, 30);
			this.flagFiendsDropRibbons.TabIndex = 5;
			this.flagFiendsDropRibbons.Text = "Fiends drop Ribbons";
			this.toolTip1.SetToolTip(this.flagFiendsDropRibbons, "Receive Ribbons for defeating each elemental dungeon boss, and remove them from s" +
        "hops and chests.");
			this.flagFiendsDropRibbons.UseVisualStyleBackColor = true;
			this.flagFiendsDropRibbons.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// flagRebalanceBosses
			// 
			this.flagRebalanceBosses.AutoSize = true;
			this.flagRebalanceBosses.Location = new System.Drawing.Point(5, 51);
			this.flagRebalanceBosses.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.flagRebalanceBosses.Name = "flagRebalanceBosses";
			this.flagRebalanceBosses.Padding = new System.Windows.Forms.Padding(3);
			this.flagRebalanceBosses.Size = new System.Drawing.Size(154, 30);
			this.flagRebalanceBosses.TabIndex = 6;
			this.flagRebalanceBosses.Text = "Rebalance bosses";
			this.toolTip1.SetToolTip(this.flagRebalanceBosses, "Increase HP of several bosses, notably Death Eye and the Fiend refights; decrease" +
        " HP of Chaos.");
			this.flagRebalanceBosses.UseVisualStyleBackColor = true;
			this.flagRebalanceBosses.CheckedChanged += new System.EventHandler(this.DetermineFlags);
			// 
			// btnRestoreVanilla
			// 
			this.btnRestoreVanilla.Location = new System.Drawing.Point(649, 508);
			this.btnRestoreVanilla.Margin = new System.Windows.Forms.Padding(2);
			this.btnRestoreVanilla.Name = "btnRestoreVanilla";
			this.btnRestoreVanilla.Padding = new System.Windows.Forms.Padding(3);
			this.btnRestoreVanilla.Size = new System.Drawing.Size(120, 33);
			this.btnRestoreVanilla.TabIndex = 27;
			this.btnRestoreVanilla.Text = "Restore vanilla";
			this.toolTip1.SetToolTip(this.btnRestoreVanilla, "Undo previous randomization and restore files to a vanilla configuration.");
			this.btnRestoreVanilla.UseVisualStyleBackColor = true;
			this.btnRestoreVanilla.Click += new System.EventHandler(this.btnRestoreVanilla_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.flagT);
			this.groupBox1.Controls.Add(this.flagTraditionalTreasure);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.RandoShop);
			this.groupBox1.Controls.Add(this.Traditional);
			this.groupBox1.Location = new System.Drawing.Point(7, 165);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(465, 117);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Items && Equipment";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.flagWandsAddInt);
			this.groupBox2.Controls.Add(this.flagRebalancePrices);
			this.groupBox2.Controls.Add(this.monsterXPGPBoost);
			this.groupBox2.Controls.Add(this.flagRestoreCritRating);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(485, 165);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox2.Size = new System.Drawing.Size(274, 201);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Balance";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.randoMagic);
			this.groupBox3.Controls.Add(this.keepMagicPermissions);
			this.groupBox3.Location = new System.Drawing.Point(7, 290);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox3.Size = new System.Drawing.Size(465, 76);
			this.groupBox3.TabIndex = 25;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Abilities";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.flagRebalanceBosses);
			this.groupBox4.Controls.Add(this.flagFiendsDropRibbons);
			this.groupBox4.Controls.Add(this.ShuffleBossSpots);
			this.groupBox4.Location = new System.Drawing.Point(7, 369);
			this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox4.Size = new System.Drawing.Size(465, 133);
			this.groupBox4.TabIndex = 26;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Monsters && Bosses";
			// 
			// FF1PRR
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 583);
			this.Controls.Add(this.btnRestoreVanilla);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.BrowseForFolder);
			this.Controls.Add(this.VisualFlags);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.NewSeed);
			this.Controls.Add(this.RandoSeed);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.NewChecksum);
			this.Controls.Add(this.RandoFlags);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.KeyItems);
			this.Controls.Add(this.CuteHats);
			this.Controls.Add(this.FF1PRFolder);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnRandomize);
			this.Name = "FF1PRR";
			this.Text = "Final Fantasy 1 Pixel Remaster Randomizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFF1PRR_FormClosing);
			this.Load += new System.EventHandler(this.FF1PRR_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRandomize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FF1PRFolder;
		private System.Windows.Forms.CheckBox CuteHats;
		private System.Windows.Forms.CheckBox ShuffleBossSpots;
		private System.Windows.Forms.CheckBox KeyItems;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox RandoFlags;
		private System.Windows.Forms.Label NewChecksum;
		private System.Windows.Forms.TextBox RandoSeed;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button NewSeed;
		private System.Windows.Forms.TextBox VisualFlags;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button BrowseForFolder;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox RandoShop;
		private System.Windows.Forms.CheckBox Traditional;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox randoMagic;
		private System.Windows.Forms.CheckBox keepMagicPermissions;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox monsterXPGPBoost;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox flagRestoreCritRating;
        private System.Windows.Forms.CheckBox flagRebalancePrices;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox flagT;
        private System.Windows.Forms.CheckBox flagTraditionalTreasure;
        private System.Windows.Forms.CheckBox flagWandsAddInt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox flagRebalanceBosses;
        private System.Windows.Forms.CheckBox flagFiendsDropRibbons;
        private System.Windows.Forms.Button btnRestoreVanilla;
    }
}
