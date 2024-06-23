
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
			components = new System.ComponentModel.Container();
			btnRandomize = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			FF1PRFolder = new System.Windows.Forms.TextBox();
			CuteHats = new System.Windows.Forms.CheckBox();
			flagBossShuffle = new System.Windows.Forms.CheckBox();
			flagKeyItems = new System.Windows.Forms.CheckBox();
			label2 = new System.Windows.Forms.Label();
			RandoFlags = new System.Windows.Forms.TextBox();
			NewChecksum = new System.Windows.Forms.Label();
			RandoSeed = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			NewSeed = new System.Windows.Forms.Button();
			VisualFlags = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			BrowseForFolder = new System.Windows.Forms.Button();
			label5 = new System.Windows.Forms.Label();
			modeShops = new System.Windows.Forms.ComboBox();
			flagShopsTrad = new System.Windows.Forms.CheckBox();
			label6 = new System.Windows.Forms.Label();
			flagMagicShuffleShops = new System.Windows.Forms.CheckBox();
			flagMagicKeepPermissions = new System.Windows.Forms.CheckBox();
			label7 = new System.Windows.Forms.Label();
			modeXPBoost = new System.Windows.Forms.ComboBox();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			label8 = new System.Windows.Forms.Label();
			modeTreasure = new System.Windows.Forms.ComboBox();
			flagTreasureTrad = new System.Windows.Forms.CheckBox();
			flagRebalancePrices = new System.Windows.Forms.CheckBox();
			flagRestoreCritRating = new System.Windows.Forms.CheckBox();
			flagWandsAddInt = new System.Windows.Forms.CheckBox();
			flagFiendsDropRibbons = new System.Windows.Forms.CheckBox();
			flagRebalanceBosses = new System.Windows.Forms.CheckBox();
			btnRestoreVanilla = new System.Windows.Forms.Button();
			flagReduceEncounterRate = new System.Windows.Forms.CheckBox();
			flagReduceChaosHP = new System.Windows.Forms.CheckBox();
			label9 = new System.Windows.Forms.Label();
			modeMagic = new System.Windows.Forms.ComboBox();
			flagNoEscapeNES = new System.Windows.Forms.CheckBox();
			flagNoEscapeRandomize = new System.Windows.Forms.CheckBox();
			label10 = new System.Windows.Forms.Label();
			modeMonsterStatAdjustment = new System.Windows.Forms.ComboBox();
			label11 = new System.Windows.Forms.Label();
			label12 = new System.Windows.Forms.Label();
			label13 = new System.Windows.Forms.Label();
			modeHeroStats = new System.Windows.Forms.ComboBox();
			flagHeroStatsStandardize = new System.Windows.Forms.CheckBox();
			statExplanation = new System.Windows.Forms.Button();
			flagBoostPromoted = new System.Windows.Forms.CheckBox();			
			characterSelection = new System.Windows.Forms.ComboBox();
			spriteSelection = new System.Windows.Forms.ComboBox();
			currentSelectionsListBox = new System.Windows.Forms.ListBox();
			includeJobUpgradeCheckBox = new System.Windows.Forms.CheckBox();
			applyButton = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			groupBox3 = new System.Windows.Forms.GroupBox();
			groupBox4 = new System.Windows.Forms.GroupBox();
			groupBox5 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox3.SuspendLayout();
			groupBox4.SuspendLayout();
			groupBox5.SuspendLayout();
			SuspendLayout();
			// 
			// btnRandomize
			// 
			btnRandomize.Location = new System.Drawing.Point(649, 654);
			btnRandomize.Margin = new System.Windows.Forms.Padding(2);
			btnRandomize.Name = "btnRandomize";
			btnRandomize.Padding = new System.Windows.Forms.Padding(3);
			btnRandomize.Size = new System.Drawing.Size(120, 33);
			btnRandomize.TabIndex = 0;
			btnRandomize.Text = "Randomize!";
			btnRandomize.UseVisualStyleBackColor = true;
			btnRandomize.Click += btnRandomize_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 12);
			label1.Margin = new System.Windows.Forms.Padding(2);
			label1.Name = "label1";
			label1.Padding = new System.Windows.Forms.Padding(3);
			label1.Size = new System.Drawing.Size(104, 26);
			label1.TabIndex = 1;
			label1.Text = "FF1 PR Folder";
			// 
			// FF1PRFolder
			// 
			FF1PRFolder.Location = new System.Drawing.Point(128, 14);
			FF1PRFolder.Margin = new System.Windows.Forms.Padding(2);
			FF1PRFolder.Name = "FF1PRFolder";
			FF1PRFolder.Size = new System.Drawing.Size(547, 27);
			FF1PRFolder.TabIndex = 2;
			FF1PRFolder.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\FINAL FANTASY PR";
			//
			// characterSelection
			//
			characterSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			characterSelection.FormattingEnabled = true;
			characterSelection.Items.AddRange(new object[] {
					"Warrior",
					"Thief",
					"Monk",
					"Red Mage",
					"White Mage",
					"Black Mage",
			});
			characterSelection.Location = new System.Drawing.Point(60, 30);
			characterSelection.Margin = new System.Windows.Forms.Padding(2);
			characterSelection.Name = "characterSelection";
			characterSelection.Size = new System.Drawing.Size(200, 28);
			characterSelection.TabIndex = 30;
			toolTip1.SetToolTip(characterSelection, "Select the character class to replace the sprite for.");
			//
			// spriteSelection
			//
			spriteSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			spriteSelection.FormattingEnabled = true;
			spriteSelection.Location = new System.Drawing.Point(60, 70);
			spriteSelection.Margin = new System.Windows.Forms.Padding(2);
			spriteSelection.Name = "spriteSelection";
			spriteSelection.Size = new System.Drawing.Size(200, 28);
			spriteSelection.TabIndex = 31;
			toolTip1.SetToolTip(spriteSelection, "Select the sprite to replace with.");
			//
			// currentSelectionsListBox
			//
			currentSelectionsListBox.FormattingEnabled = true;
			currentSelectionsListBox.Location = new System.Drawing.Point(10, 150);
			currentSelectionsListBox.Margin = new System.Windows.Forms.Padding(2);
			currentSelectionsListBox.Name = "currentSelectionsListBox";
			currentSelectionsListBox.Size = new System.Drawing.Size(250, 140);
			currentSelectionsListBox.TabIndex = 32;
			//
			// applyButton
			//
			applyButton.Location = new System.Drawing.Point(180, 108);
			applyButton.Margin = new System.Windows.Forms.Padding(2);
			applyButton.Name = "applyButton";
			applyButton.Size = new System.Drawing.Size(80, 32);
			applyButton.TabIndex = 33;
			applyButton.Text = "Apply";
			applyButton.UseVisualStyleBackColor = true;
			applyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			//
			// includeJobUpgradeCheckBox
			//
			includeJobUpgradeCheckBox.AutoSize = true;
			includeJobUpgradeCheckBox.Location = new System.Drawing.Point(10, 112);
			includeJobUpgradeCheckBox.Margin = new System.Windows.Forms.Padding(2);
			includeJobUpgradeCheckBox.Name = "includeJobUpgradeCheckBox";
			includeJobUpgradeCheckBox.Size = new System.Drawing.Size(160, 24);
			includeJobUpgradeCheckBox.TabIndex = 34;
			includeJobUpgradeCheckBox.Text = "Include Job Upgrade";
			includeJobUpgradeCheckBox.UseVisualStyleBackColor = true;
			toolTip1.SetToolTip(includeJobUpgradeCheckBox, "If checked, the sprite change will also apply to the upgraded job.");
			// 
			// CuteHats
			// 
			CuteHats.AutoSize = true;
			CuteHats.Location = new System.Drawing.Point(485, 128);
			CuteHats.Margin = new System.Windows.Forms.Padding(2);
			CuteHats.Name = "CuteHats";
			CuteHats.Padding = new System.Windows.Forms.Padding(3);
			CuteHats.Size = new System.Drawing.Size(101, 30);
			CuteHats.TabIndex = 3;
			CuteHats.Text = "Cute Hats";
			toolTip1.SetToolTip(CuteHats, "Your hat is cute.");
			CuteHats.UseVisualStyleBackColor = true;
			CuteHats.Visible = false;
			CuteHats.Click += DetermineFlags;
			// 
			// flagBossShuffle
			// 
			flagBossShuffle.AutoSize = true;
			flagBossShuffle.Enabled = false;
			flagBossShuffle.Location = new System.Drawing.Point(172, 22);
			flagBossShuffle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagBossShuffle.Name = "flagBossShuffle";
			flagBossShuffle.Padding = new System.Windows.Forms.Padding(3);
			flagBossShuffle.Size = new System.Drawing.Size(158, 30);
			flagBossShuffle.TabIndex = 4;
			flagBossShuffle.Text = "Shuffle Boss Spots";
			toolTip1.SetToolTip(flagBossShuffle, "Change which boss appears at which boss location.");
			flagBossShuffle.UseVisualStyleBackColor = true;
			flagBossShuffle.Visible = false;
			flagBossShuffle.CheckedChanged += DetermineFlags;
			// 
			// flagKeyItems
			// 
			flagKeyItems.AutoSize = true;
			flagKeyItems.Checked = true;
			flagKeyItems.CheckState = System.Windows.Forms.CheckState.Checked;
			flagKeyItems.Location = new System.Drawing.Point(12, 128);
			flagKeyItems.Margin = new System.Windows.Forms.Padding(2);
			flagKeyItems.Name = "flagKeyItems";
			flagKeyItems.Padding = new System.Windows.Forms.Padding(3);
			flagKeyItems.Size = new System.Drawing.Size(180, 30);
			flagKeyItems.TabIndex = 5;
			flagKeyItems.Text = "Randomize &Key Items";
			toolTip1.SetToolTip(flagKeyItems, "Change which key item appears at each location. All key items are guaranteed to be accessible.");
			flagKeyItems.UseVisualStyleBackColor = true;
			flagKeyItems.CheckedChanged += DetermineFlags;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 51);
			label2.Margin = new System.Windows.Forms.Padding(2);
			label2.Name = "label2";
			label2.Padding = new System.Windows.Forms.Padding(3);
			label2.Size = new System.Drawing.Size(120, 26);
			label2.TabIndex = 6;
			label2.Text = "Gameplay Flags";
			// 
			// RandoFlags
			// 
			RandoFlags.Location = new System.Drawing.Point(128, 52);
			RandoFlags.Margin = new System.Windows.Forms.Padding(2);
			RandoFlags.Name = "RandoFlags";
			RandoFlags.Size = new System.Drawing.Size(346, 27);
			RandoFlags.TabIndex = 7;
			// 
			// NewChecksum
			// 
			NewChecksum.AutoSize = true;
			NewChecksum.Location = new System.Drawing.Point(12, 658);
			NewChecksum.Margin = new System.Windows.Forms.Padding(2);
			NewChecksum.Name = "NewChecksum";
			NewChecksum.Padding = new System.Windows.Forms.Padding(3);
			NewChecksum.Size = new System.Drawing.Size(273, 26);
			NewChecksum.TabIndex = 8;
			NewChecksum.Text = "New Checksum:  (Not Randomized Yet)";
			// 
			// RandoSeed
			// 
			RandoSeed.Location = new System.Drawing.Point(542, 52);
			RandoSeed.Margin = new System.Windows.Forms.Padding(2);
			RandoSeed.Name = "RandoSeed";
			RandoSeed.Size = new System.Drawing.Size(154, 27);
			RandoSeed.TabIndex = 10;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(485, 51);
			label3.Margin = new System.Windows.Forms.Padding(2);
			label3.Name = "label3";
			label3.Padding = new System.Windows.Forms.Padding(3);
			label3.Size = new System.Drawing.Size(48, 26);
			label3.TabIndex = 9;
			label3.Text = "Seed";
			// 
			// NewSeed
			// 
			NewSeed.Location = new System.Drawing.Point(710, 50);
			NewSeed.Margin = new System.Windows.Forms.Padding(2);
			NewSeed.Name = "NewSeed";
			NewSeed.Padding = new System.Windows.Forms.Padding(3);
			NewSeed.Size = new System.Drawing.Size(59, 33);
			NewSeed.TabIndex = 11;
			NewSeed.Text = "New";
			NewSeed.UseVisualStyleBackColor = true;
			NewSeed.Click += NewSeed_Click;
			// 
			// VisualFlags
			// 
			VisualFlags.Location = new System.Drawing.Point(128, 91);
			VisualFlags.Margin = new System.Windows.Forms.Padding(2);
			VisualFlags.Name = "VisualFlags";
			VisualFlags.Size = new System.Drawing.Size(346, 27);
			VisualFlags.TabIndex = 13;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(12, 89);
			label4.Margin = new System.Windows.Forms.Padding(2);
			label4.Name = "label4";
			label4.Padding = new System.Windows.Forms.Padding(3);
			label4.Size = new System.Drawing.Size(114, 26);
			label4.TabIndex = 12;
			label4.Text = "Cosmetic Flags";
			// 
			// BrowseForFolder
			// 
			BrowseForFolder.Location = new System.Drawing.Point(686, 12);
			BrowseForFolder.Margin = new System.Windows.Forms.Padding(2);
			BrowseForFolder.Name = "BrowseForFolder";
			BrowseForFolder.Padding = new System.Windows.Forms.Padding(3);
			BrowseForFolder.Size = new System.Drawing.Size(82, 32);
			BrowseForFolder.TabIndex = 14;
			BrowseForFolder.Text = "Browse";
			BrowseForFolder.UseVisualStyleBackColor = true;
			BrowseForFolder.Click += btnBrowse_Click;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(8, 28);
			label5.Margin = new System.Windows.Forms.Padding(6);
			label5.Name = "label5";
			label5.Padding = new System.Windows.Forms.Padding(3);
			label5.Size = new System.Drawing.Size(58, 26);
			label5.TabIndex = 15;
			label5.Text = "&Shops:";
			toolTip1.SetToolTip(label5, "Randomize shop contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			// 
			// modeShops
			// 
			modeShops.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeShops.FormattingEnabled = true;
			modeShops.Items.AddRange(new object[] { "None", "Shuffle" });
			modeShops.Location = new System.Drawing.Point(121, 26);
			modeShops.Margin = new System.Windows.Forms.Padding(6);
			modeShops.Name = "modeShops";
			modeShops.Size = new System.Drawing.Size(151, 28);
			modeShops.TabIndex = 16;
			toolTip1.SetToolTip(modeShops, "Randomize shop contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			modeShops.SelectedIndexChanged += DetermineFlags;
			// 
			// flagShopsTrad
			// 
			flagShopsTrad.AutoSize = true;
			flagShopsTrad.Location = new System.Drawing.Point(282, 24);
			flagShopsTrad.Margin = new System.Windows.Forms.Padding(6);
			flagShopsTrad.Name = "flagShopsTrad";
			flagShopsTrad.Padding = new System.Windows.Forms.Padding(3);
			flagShopsTrad.Size = new System.Drawing.Size(160, 30);
			flagShopsTrad.TabIndex = 17;
			flagShopsTrad.Text = "E&xclude DoS items";
			toolTip1.SetToolTip(flagShopsTrad, "Remove newer items (Ether, Phoenix Down, etc) from shops.");
			flagShopsTrad.UseVisualStyleBackColor = true;
			flagShopsTrad.CheckedChanged += DetermineFlags;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(12, 262);
			label6.Margin = new System.Windows.Forms.Padding(2);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(0, 20);
			label6.TabIndex = 18;
			// 
			// flagMagicShuffleShops
			// 
			flagMagicShuffleShops.AutoSize = true;
			flagMagicShuffleShops.Location = new System.Drawing.Point(11, 60);
			flagMagicShuffleShops.Margin = new System.Windows.Forms.Padding(6);
			flagMagicShuffleShops.Name = "flagMagicShuffleShops";
			flagMagicShuffleShops.Padding = new System.Windows.Forms.Padding(3);
			flagMagicShuffleShops.Size = new System.Drawing.Size(127, 30);
			flagMagicShuffleShops.TabIndex = 19;
			flagMagicShuffleShops.Text = "Shuffle Shops";
			toolTip1.SetToolTip(flagMagicShuffleShops, "Change which spell level is available at each shop.");
			flagMagicShuffleShops.UseVisualStyleBackColor = true;
			flagMagicShuffleShops.CheckedChanged += DetermineFlags;
			// 
			// flagMagicKeepPermissions
			// 
			flagMagicKeepPermissions.AutoSize = true;
			flagMagicKeepPermissions.Location = new System.Drawing.Point(150, 59);
			flagMagicKeepPermissions.Margin = new System.Windows.Forms.Padding(6);
			flagMagicKeepPermissions.Name = "flagMagicKeepPermissions";
			flagMagicKeepPermissions.Padding = new System.Windows.Forms.Padding(3);
			flagMagicKeepPermissions.Size = new System.Drawing.Size(151, 30);
			flagMagicKeepPermissions.TabIndex = 20;
			flagMagicKeepPermissions.Text = "&Keep Permissions";
			toolTip1.SetToolTip(flagMagicKeepPermissions, "Preserve who may learn each spell, rather than adjusting to the spell's new slot. (eg, level 1 Flare can still only be learned by Black Wizard)");
			flagMagicKeepPermissions.UseVisualStyleBackColor = true;
			flagMagicKeepPermissions.CheckedChanged += DetermineFlags;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(5, 24);
			label7.Name = "label7";
			label7.Padding = new System.Windows.Forms.Padding(3);
			label7.Size = new System.Drawing.Size(98, 26);
			label7.TabIndex = 21;
			label7.Text = "&XP/Gil Boost";
			toolTip1.SetToolTip(label7, "How much to increase earned XP and Gil.");
			// 
			// modeXPBoost
			// 
			modeXPBoost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeXPBoost.FormattingEnabled = true;
			modeXPBoost.Items.AddRange(new object[] { "0.5x", "1.0x", "1.5x", "2.0x", "3.0x", "4.0x", "5.0x", "10.0x" });
			modeXPBoost.Location = new System.Drawing.Point(106, 20);
			modeXPBoost.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			modeXPBoost.Name = "modeXPBoost";
			modeXPBoost.Size = new System.Drawing.Size(69, 30);
			modeXPBoost.TabIndex = 22;
			toolTip1.SetToolTip(modeXPBoost, "How much to increase earned XP and Gil.");
			modeXPBoost.SelectedIndexChanged += DetermineFlags;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(8, 67);
			label8.Margin = new System.Windows.Forms.Padding(6);
			label8.Name = "label8";
			label8.Padding = new System.Windows.Forms.Padding(3);
			label8.Size = new System.Drawing.Size(73, 26);
			label8.TabIndex = 18;
			label8.Text = "&Treasure:";
			toolTip1.SetToolTip(label8, "Randomize treasure chest contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			// 
			// modeTreasure
			// 
			modeTreasure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeTreasure.FormattingEnabled = true;
			modeTreasure.Items.AddRange(new object[] { "None", "Shuffle" });
			modeTreasure.Location = new System.Drawing.Point(121, 65);
			modeTreasure.Margin = new System.Windows.Forms.Padding(6);
			modeTreasure.Name = "modeTreasure";
			modeTreasure.Size = new System.Drawing.Size(151, 28);
			modeTreasure.TabIndex = 19;
			toolTip1.SetToolTip(modeTreasure, "Randomize treasure chest contents. None: . Shuffle: . Standard: . Pro: . Wild: .");
			modeTreasure.SelectedIndexChanged += DetermineFlags;
			// 
			// flagTreasureTrad
			// 
			flagTreasureTrad.AutoSize = true;
			flagTreasureTrad.Enabled = false;
			flagTreasureTrad.Location = new System.Drawing.Point(282, 63);
			flagTreasureTrad.Margin = new System.Windows.Forms.Padding(6);
			flagTreasureTrad.Name = "flagTreasureTrad";
			flagTreasureTrad.Padding = new System.Windows.Forms.Padding(3);
			flagTreasureTrad.Size = new System.Drawing.Size(160, 30);
			flagTreasureTrad.TabIndex = 20;
			flagTreasureTrad.Text = "E&xclude DoS items";
			toolTip1.SetToolTip(flagTreasureTrad, "Remove newer items (Ether, Phoenix Down, etc) from shops.");
			flagTreasureTrad.UseVisualStyleBackColor = true;
			flagTreasureTrad.CheckedChanged += DetermineFlags;
			// 
			// flagRebalancePrices
			// 
			flagRebalancePrices.AutoSize = true;
			flagRebalancePrices.Location = new System.Drawing.Point(6, 90);
			flagRebalancePrices.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagRebalancePrices.Name = "flagRebalancePrices";
			flagRebalancePrices.Padding = new System.Windows.Forms.Padding(3);
			flagRebalancePrices.Size = new System.Drawing.Size(183, 30);
			flagRebalancePrices.TabIndex = 21;
			flagRebalancePrices.Text = "Rebalance item prices";
			toolTip1.SetToolTip(flagRebalancePrices, "Adjust item prices for a more balanced experience. Increases price of HP/MP restorative items and Phoenix Down.");
			flagRebalancePrices.UseVisualStyleBackColor = true;
			flagRebalancePrices.CheckedChanged += DetermineFlags;
			// 
			// flagRestoreCritRating
			// 
			flagRestoreCritRating.AutoSize = true;
			flagRestoreCritRating.Location = new System.Drawing.Point(6, 60);
			flagRestoreCritRating.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			flagRestoreCritRating.Name = "flagRestoreCritRating";
			flagRestoreCritRating.Padding = new System.Windows.Forms.Padding(3);
			flagRestoreCritRating.Size = new System.Drawing.Size(155, 30);
			flagRestoreCritRating.TabIndex = 22;
			flagRestoreCritRating.Text = "Restore crit rating";
			toolTip1.SetToolTip(flagRestoreCritRating, "Weapon critical rate changed to reflect original NES data.");
			flagRestoreCritRating.UseVisualStyleBackColor = true;
			flagRestoreCritRating.CheckedChanged += DetermineFlags;
			// 
			// flagWandsAddInt
			// 
			flagWandsAddInt.AutoSize = true;
			flagWandsAddInt.Location = new System.Drawing.Point(6, 120);
			flagWandsAddInt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			flagWandsAddInt.Name = "flagWandsAddInt";
			flagWandsAddInt.Padding = new System.Windows.Forms.Padding(3);
			flagWandsAddInt.Size = new System.Drawing.Size(138, 30);
			flagWandsAddInt.TabIndex = 23;
			flagWandsAddInt.Text = "Wands add INT";
			toolTip1.SetToolTip(flagWandsAddInt, "Staves and Hammers increase the user's Intelligence when equipped.");
			flagWandsAddInt.UseVisualStyleBackColor = true;
			flagWandsAddInt.CheckedChanged += DetermineFlags;
			// 
			// flagFiendsDropRibbons
			// 
			flagFiendsDropRibbons.AutoSize = true;
			flagFiendsDropRibbons.Location = new System.Drawing.Point(5, 80);
			flagFiendsDropRibbons.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagFiendsDropRibbons.Name = "flagFiendsDropRibbons";
			flagFiendsDropRibbons.Padding = new System.Windows.Forms.Padding(3);
			flagFiendsDropRibbons.Size = new System.Drawing.Size(173, 30);
			flagFiendsDropRibbons.TabIndex = 5;
			flagFiendsDropRibbons.Text = "Fiends drop Ribbons";
			toolTip1.SetToolTip(flagFiendsDropRibbons, "Receive Ribbons for defeating each elemental dungeon boss, and remove them from shops and chests.");
			flagFiendsDropRibbons.UseVisualStyleBackColor = true;
			flagFiendsDropRibbons.CheckedChanged += DetermineFlags;
			// 
			// flagRebalanceBosses
			// 
			flagRebalanceBosses.AutoSize = true;
			flagRebalanceBosses.Location = new System.Drawing.Point(7, 52);
			flagRebalanceBosses.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			flagRebalanceBosses.Name = "flagRebalanceBosses";
			flagRebalanceBosses.Padding = new System.Windows.Forms.Padding(3);
			flagRebalanceBosses.Size = new System.Drawing.Size(131, 30);
			flagRebalanceBosses.TabIndex = 6;
			flagRebalanceBosses.Text = "Harder bosses";
			toolTip1.SetToolTip(flagRebalanceBosses, "Increase HP of several bosses, notably Death Eye and the Fiend refights.");
			flagRebalanceBosses.UseVisualStyleBackColor = true;
			flagRebalanceBosses.CheckedChanged += DetermineFlags;
			// 
			// btnRestoreVanilla
			// 
			btnRestoreVanilla.Location = new System.Drawing.Point(520, 654);
			btnRestoreVanilla.Margin = new System.Windows.Forms.Padding(2);
			btnRestoreVanilla.Name = "btnRestoreVanilla";
			btnRestoreVanilla.Padding = new System.Windows.Forms.Padding(3);
			btnRestoreVanilla.Size = new System.Drawing.Size(120, 33);
			btnRestoreVanilla.TabIndex = 27;
			btnRestoreVanilla.Text = "Restore vanilla";
			toolTip1.SetToolTip(btnRestoreVanilla, "Undo previous randomization and restore files to a vanilla configuration.");
			btnRestoreVanilla.UseVisualStyleBackColor = true;
			btnRestoreVanilla.Click += btnRestoreVanilla_Click;
			// 
			// flagReduceEncounterRate
			// 
			flagReduceEncounterRate.AutoSize = true;
			flagReduceEncounterRate.Location = new System.Drawing.Point(6, 150);
			flagReduceEncounterRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			flagReduceEncounterRate.Name = "flagReduceEncounterRate";
			flagReduceEncounterRate.Padding = new System.Windows.Forms.Padding(3);
			flagReduceEncounterRate.Size = new System.Drawing.Size(186, 30);
			flagReduceEncounterRate.TabIndex = 24;
			flagReduceEncounterRate.Text = "Reduce encounter rate";
			toolTip1.SetToolTip(flagReduceEncounterRate, "Reduces rate of random encounters significantly on the ocean and slightly everywhere else.");
			flagReduceEncounterRate.UseVisualStyleBackColor = true;
			flagReduceEncounterRate.CheckedChanged += DetermineFlags;
			// 
			// flagReduceChaosHP
			// 
			flagReduceChaosHP.AutoSize = true;
			flagReduceChaosHP.Location = new System.Drawing.Point(6, 22);
			flagReduceChaosHP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagReduceChaosHP.Name = "flagReduceChaosHP";
			flagReduceChaosHP.Padding = new System.Windows.Forms.Padding(3);
			flagReduceChaosHP.Size = new System.Drawing.Size(153, 30);
			flagReduceChaosHP.TabIndex = 7;
			flagReduceChaosHP.Text = "Reduce Chaos HP";
			toolTip1.SetToolTip(flagReduceChaosHP, "Decrease HP of Chaos to 9600.");
			flagReduceChaosHP.UseVisualStyleBackColor = true;
			flagReduceChaosHP.CheckedChanged += DetermineFlags;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(8, 30);
			label9.Margin = new System.Windows.Forms.Padding(6);
			label9.Name = "label9";
			label9.Padding = new System.Windows.Forms.Padding(3);
			label9.Size = new System.Drawing.Size(59, 26);
			label9.TabIndex = 21;
			label9.Text = "&Magic:";
			toolTip1.SetToolTip(label9, "Randomize magic spells. None: . Standard: . Pro: . Wild: . Chaos: .");
			// 
			// modeMagic
			// 
			modeMagic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeMagic.FormattingEnabled = true;
			modeMagic.Items.AddRange(new object[] { "None", "Standard", "Pro", "Wild", "Chaos" });
			modeMagic.Location = new System.Drawing.Point(121, 28);
			modeMagic.Margin = new System.Windows.Forms.Padding(6);
			modeMagic.Name = "modeMagic";
			modeMagic.Size = new System.Drawing.Size(151, 28);
			modeMagic.TabIndex = 22;
			toolTip1.SetToolTip(modeMagic, "Randomize magic spells. None: . Standard: . Pro: . Wild: . Chaos: .");
			modeMagic.SelectedIndexChanged += DetermineFlags;
			// 
			// flagNoEscapeNES
			// 
			flagNoEscapeNES.AutoSize = true;
			flagNoEscapeNES.Location = new System.Drawing.Point(172, 51);
			flagNoEscapeNES.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagNoEscapeNES.Name = "flagNoEscapeNES";
			flagNoEscapeNES.Padding = new System.Windows.Forms.Padding(3);
			flagNoEscapeNES.Size = new System.Drawing.Size(291, 30);
			flagNoEscapeNES.TabIndex = 8;
			flagNoEscapeNES.Text = "Add NES \"No Escape\" Rnd. Encounters";
			toolTip1.SetToolTip(flagNoEscapeNES, "Change which boss appears at which boss location.");
			flagNoEscapeNES.UseVisualStyleBackColor = true;
			flagNoEscapeNES.CheckedChanged += DetermineFlags;
			// 
			// flagNoEscapeRandomize
			// 
			flagNoEscapeRandomize.AutoSize = true;
			flagNoEscapeRandomize.Location = new System.Drawing.Point(172, 80);
			flagNoEscapeRandomize.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			flagNoEscapeRandomize.Name = "flagNoEscapeRandomize";
			flagNoEscapeRandomize.Padding = new System.Windows.Forms.Padding(3);
			flagNoEscapeRandomize.Size = new System.Drawing.Size(274, 30);
			flagNoEscapeRandomize.TabIndex = 9;
			flagNoEscapeRandomize.Text = "Randomize \"No Escape\" Encounters";
			toolTip1.SetToolTip(flagNoEscapeRandomize, "Change which boss appears at which boss location.");
			flagNoEscapeRandomize.UseVisualStyleBackColor = true;
			flagNoEscapeRandomize.CheckedChanged += DetermineFlags;
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(5, 114);
			label10.Name = "label10";
			label10.Padding = new System.Windows.Forms.Padding(3);
			label10.Size = new System.Drawing.Size(121, 26);
			label10.TabIndex = 22;
			label10.Text = "Stat Adjustment";
			toolTip1.SetToolTip(label10, "How much to increase earned XP and Gil.");
			// 
			// modeMonsterStatAdjustment
			// 
			modeMonsterStatAdjustment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeMonsterStatAdjustment.FormattingEnabled = true;
			modeMonsterStatAdjustment.Items.AddRange(new object[] { "100%", "66%-150%", "50%-200%", "33%-300%", "25%-400%", "20%-500%", "100%-125%", "100%-150%", "100%-200%", "100%-300%", "100%-400%", "100%-500%" });
			modeMonsterStatAdjustment.Location = new System.Drawing.Point(138, 114);
			modeMonsterStatAdjustment.Margin = new System.Windows.Forms.Padding(6);
			modeMonsterStatAdjustment.Name = "modeMonsterStatAdjustment";
			modeMonsterStatAdjustment.Size = new System.Drawing.Size(151, 28);
			modeMonsterStatAdjustment.TabIndex = 23;
			toolTip1.SetToolTip(modeMonsterStatAdjustment, "Randomize magic spells. None: . Standard: . Pro: . Wild: . Chaos: .");
			modeMonsterStatAdjustment.SelectedIndexChanged += DetermineFlags;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(6, 95);
			label11.Margin = new System.Windows.Forms.Padding(6);
			label11.Name = "label11";
			label11.Padding = new System.Windows.Forms.Padding(3);
			label11.Size = new System.Drawing.Size(87, 26);
			label11.TabIndex = 23;
			label11.Text = "Hero Stats:";
			toolTip1.SetToolTip(label11, "Randomize magic spells. None: . Standard: . Pro: . Wild: . Chaos: .");
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(10, 30);
			label12.Margin = new System.Windows.Forms.Padding(6);
			label12.Name = "label12";
			label12.Padding = new System.Windows.Forms.Padding(3);
			label12.Size = new System.Drawing.Size(87, 26);
			label12.TabIndex = 38;
			label12.Text = "Job:";
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(4, 70);
			label13.Margin = new System.Windows.Forms.Padding(6);
			label13.Name = "label13";
			label13.Padding = new System.Windows.Forms.Padding(3);
			label13.Size = new System.Drawing.Size(87, 26);
			label13.TabIndex = 38;
			label13.Text = "Sprite:";
			// 
			// modeHeroStats
			// 
			modeHeroStats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			modeHeroStats.FormattingEnabled = true;
			modeHeroStats.Items.AddRange(new object[] { "None", "Shuffle", "Standard", "Silly", "Wild", "Chaos" });
			modeHeroStats.Location = new System.Drawing.Point(121, 94);
			modeHeroStats.Margin = new System.Windows.Forms.Padding(6);
			modeHeroStats.Name = "modeHeroStats";
			modeHeroStats.Size = new System.Drawing.Size(151, 28);
			modeHeroStats.TabIndex = 24;
			toolTip1.SetToolTip(modeHeroStats, "Randomize magic spells. None: . Standard: . Pro: . Wild: . Chaos: .");
			modeHeroStats.SelectedIndexChanged += DetermineFlags;
			// 
			// flagHeroStatsStandardize
			// 
			flagHeroStatsStandardize.AutoSize = true;
			flagHeroStatsStandardize.Location = new System.Drawing.Point(121, 131);
			flagHeroStatsStandardize.Margin = new System.Windows.Forms.Padding(6);
			flagHeroStatsStandardize.Name = "flagHeroStatsStandardize";
			flagHeroStatsStandardize.Padding = new System.Windows.Forms.Padding(3);
			flagHeroStatsStandardize.Size = new System.Drawing.Size(116, 30);
			flagHeroStatsStandardize.TabIndex = 25;
			flagHeroStatsStandardize.Text = "Standardize";
			toolTip1.SetToolTip(flagHeroStatsStandardize, "Preserve who may learn each spell, rather than adjusting to the spell's new slot. (eg, level 1 Flare can still only be learned by Black Wizard)");
			flagHeroStatsStandardize.UseVisualStyleBackColor = true;
			flagHeroStatsStandardize.CheckedChanged += DetermineFlags;
			// 
			// statExplanation
			// 
			statExplanation.Location = new System.Drawing.Point(280, 93);
			statExplanation.Margin = new System.Windows.Forms.Padding(2);
			statExplanation.Name = "statExplanation";
			statExplanation.Padding = new System.Windows.Forms.Padding(3);
			statExplanation.Size = new System.Drawing.Size(28, 33);
			statExplanation.TabIndex = 28;
			statExplanation.Text = "?";
			toolTip1.SetToolTip(statExplanation, "Undo previous randomization and restore files to a vanilla configuration.");
			statExplanation.UseVisualStyleBackColor = true;
			statExplanation.Click += statExplanation_Click;
			// 
			// flagBoostPromoted
			// 
			flagBoostPromoted.AutoSize = true;
			flagBoostPromoted.Location = new System.Drawing.Point(249, 131);
			flagBoostPromoted.Margin = new System.Windows.Forms.Padding(6);
			flagBoostPromoted.Name = "flagBoostPromoted";
			flagBoostPromoted.Padding = new System.Windows.Forms.Padding(3);
			flagBoostPromoted.Size = new System.Drawing.Size(195, 30);
			flagBoostPromoted.TabIndex = 29;
			flagBoostPromoted.Text = "Boost promoted classes";
			toolTip1.SetToolTip(flagBoostPromoted, "Preserve who may learn each spell, rather than adjusting to the spell's new slot. (eg, level 1 Flare can still only be learned by Black Wizard)");
			flagBoostPromoted.UseVisualStyleBackColor = true;
			flagBoostPromoted.CheckedChanged += DetermineFlags;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(label8);
			groupBox1.Controls.Add(modeTreasure);
			groupBox1.Controls.Add(flagTreasureTrad);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(modeShops);
			groupBox1.Controls.Add(flagShopsTrad);
			groupBox1.Location = new System.Drawing.Point(7, 165);
			groupBox1.Margin = new System.Windows.Forms.Padding(2);
			groupBox1.Name = "groupBox1";
			groupBox1.Padding = new System.Windows.Forms.Padding(2);
			groupBox1.Size = new System.Drawing.Size(465, 117);
			groupBox1.TabIndex = 23;
			groupBox1.TabStop = false;
			groupBox1.Text = "Items && Equipment";
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(flagReduceEncounterRate);
			groupBox2.Controls.Add(flagWandsAddInt);
			groupBox2.Controls.Add(flagRebalancePrices);
			groupBox2.Controls.Add(modeXPBoost);
			groupBox2.Controls.Add(flagRestoreCritRating);
			groupBox2.Controls.Add(label7);
			groupBox2.Location = new System.Drawing.Point(485, 125);
			groupBox2.Margin = new System.Windows.Forms.Padding(2);
			groupBox2.Name = "groupBox2";
			groupBox2.Padding = new System.Windows.Forms.Padding(2);
			groupBox2.Size = new System.Drawing.Size(274, 200);
			groupBox2.TabIndex = 24;
			groupBox2.TabStop = false;
			groupBox2.Text = "Balance";
			// 
			// groupBox3
			// 
			groupBox3.Controls.Add(flagBoostPromoted);
			groupBox3.Controls.Add(statExplanation);
			groupBox3.Controls.Add(flagHeroStatsStandardize);
			groupBox3.Controls.Add(modeHeroStats);
			groupBox3.Controls.Add(label11);
			groupBox3.Controls.Add(label9);
			groupBox3.Controls.Add(flagMagicShuffleShops);
			groupBox3.Controls.Add(modeMagic);
			groupBox3.Controls.Add(flagMagicKeepPermissions);
			groupBox3.Location = new System.Drawing.Point(7, 290);
			groupBox3.Margin = new System.Windows.Forms.Padding(2);
			groupBox3.Name = "groupBox3";
			groupBox3.Padding = new System.Windows.Forms.Padding(2);
			groupBox3.Size = new System.Drawing.Size(465, 169);
			groupBox3.TabIndex = 25;
			groupBox3.TabStop = false;
			groupBox3.Text = "Abilities";
			// 
			// groupBox4
			// 
			groupBox4.Controls.Add(modeMonsterStatAdjustment);
			groupBox4.Controls.Add(label10);
			groupBox4.Controls.Add(flagNoEscapeRandomize);
			groupBox4.Controls.Add(flagNoEscapeNES);
			groupBox4.Controls.Add(flagReduceChaosHP);
			groupBox4.Controls.Add(flagRebalanceBosses);
			groupBox4.Controls.Add(flagFiendsDropRibbons);
			groupBox4.Controls.Add(flagBossShuffle);
			groupBox4.Location = new System.Drawing.Point(7, 478);
			groupBox4.Margin = new System.Windows.Forms.Padding(2);
			groupBox4.Name = "groupBox4";
			groupBox4.Padding = new System.Windows.Forms.Padding(2);
			groupBox4.Size = new System.Drawing.Size(465, 165);
			groupBox4.TabIndex = 26;
			groupBox4.TabStop = false;
			groupBox4.Text = "Monsters && Bosses";
			// 
			// groupBox5
			//
			groupBox5.Controls.Add(characterSelection);
			groupBox5.Controls.Add(spriteSelection);
			groupBox5.Controls.Add(currentSelectionsListBox);
			groupBox5.Controls.Add(applyButton);
			groupBox5.Controls.Add(includeJobUpgradeCheckBox);
			groupBox5.Controls.Add(label12);
			groupBox5.Controls.Add(label13);
			groupBox5.Location = new System.Drawing.Point(485, 330);
			groupBox5.Name = "groupBox5";
			groupBox5.Size = new System.Drawing.Size(274, 312);
			groupBox5.TabIndex = 31;
			groupBox5.TabStop = false;
			groupBox5.Text = "Overworld Sprites";
			// 
			// FF1PRR
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(780, 694);
			Controls.Add(btnRestoreVanilla);
			Controls.Add(groupBox5);
			Controls.Add(groupBox4);
			Controls.Add(groupBox3);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Controls.Add(label6);
			Controls.Add(BrowseForFolder);
			Controls.Add(VisualFlags);
			Controls.Add(label4);
			Controls.Add(NewSeed);
			Controls.Add(RandoSeed);
			Controls.Add(label3);
			Controls.Add(NewChecksum);
			Controls.Add(RandoFlags);
			Controls.Add(label2);
			Controls.Add(flagKeyItems);
			Controls.Add(CuteHats);
			Controls.Add(FF1PRFolder);
			Controls.Add(label1);
			Controls.Add(btnRandomize);
			Name = "FF1PRR";
			Text = "Final Fantasy 1 Pixel Remaster Randomizer";
			FormClosing += frmFF1PRR_FormClosing;
			Load += FF1PRR_Load;
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			groupBox4.ResumeLayout(false);
			groupBox4.PerformLayout();
			groupBox5.ResumeLayout(false);
			groupBox5.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnRandomize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FF1PRFolder;
		private System.Windows.Forms.CheckBox CuteHats;
		private System.Windows.Forms.CheckBox flagBossShuffle;
		private System.Windows.Forms.CheckBox flagKeyItems;
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
		private System.Windows.Forms.ComboBox modeShops;
		private System.Windows.Forms.CheckBox flagShopsTrad;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox flagMagicShuffleShops;
		private System.Windows.Forms.CheckBox flagMagicKeepPermissions;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox modeXPBoost;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox flagRestoreCritRating;
        private System.Windows.Forms.CheckBox flagRebalancePrices;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox modeTreasure;
        private System.Windows.Forms.CheckBox flagTreasureTrad;
        private System.Windows.Forms.CheckBox flagWandsAddInt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox flagRebalanceBosses;
        private System.Windows.Forms.CheckBox flagFiendsDropRibbons;
        private System.Windows.Forms.Button btnRestoreVanilla;
        private System.Windows.Forms.CheckBox flagReduceEncounterRate;
        private System.Windows.Forms.CheckBox flagReduceChaosHP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox modeMagic;
		private System.Windows.Forms.CheckBox flagNoEscapeRandomize;
		private System.Windows.Forms.CheckBox flagNoEscapeNES;
		private System.Windows.Forms.ComboBox modeMonsterStatAdjustment;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox flagHeroStatsStandardize;
		private System.Windows.Forms.ComboBox modeHeroStats;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button statExplanation;
		private System.Windows.Forms.CheckBox flagBoostPromoted;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ComboBox characterSelection;
		private System.Windows.Forms.ComboBox spriteSelection;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.ListBox currentSelectionsListBox;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox includeJobUpgradeCheckBox;
	}
}

